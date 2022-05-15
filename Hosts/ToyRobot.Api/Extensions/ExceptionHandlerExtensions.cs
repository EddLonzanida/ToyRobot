using ToyRobot.Infrastructure;
using ToyRobot.Infrastructure.Extensions;
using Eml.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Generic;
using System.Net;

namespace ToyRobot.Api.Extensions
{
	public static class ExceptionHandlerExtensions
	{
		public static string GetLogDetails(this HttpRequest request)
		{
			var logDetails = new
			{
				request.RouteValues,
				request.Headers,
				request.Method,
				request.Host
			};

			return logDetails.Serialize();
		}

		/// <summary>
		/// Used to add more details when debugging.
		/// </summary>
		public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app, string environmentName)
		{
			return app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					const int STATUS_CODE = (int)HttpStatusCode.InternalServerError;

					context.Response.StatusCode = STATUS_CODE;
					context.Response.ContentType = "application/json";

					var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

					if (exceptionHandlerFeature != null)
					{
						var problemDetails = new Dictionary<string, object>();
						var isProduction = environmentName.IsProductionEnvironment();

						// Do not show error details in the production.
						if (isProduction)
						{
							problemDetails.Add("Name", "Internal server error.");
							problemDetails.Add("Status", context.Response.StatusCode);
						}
						else
						{
							var request = context.Request;
							var body = await context.GetRequestBody();
							var method = request.Method;
							var url = request.GetDisplayUrl();
							var errorMessage = exceptionHandlerFeature.Error.Message;
							var stackTrace = exceptionHandlerFeature.Error.StackTrace;

							problemDetails.Add("Name", errorMessage);
							problemDetails.Add("Status", context.Response.StatusCode);
							problemDetails.Add("Environment", environmentName);
							problemDetails.Add("ReleaseVersion", AppConstants.ReleaseVersion);
							problemDetails.Add("Method", method);
							problemDetails.Add("Url", url);

							if (body != null) problemDetails.Add("Body", body);

							if (stackTrace != null) problemDetails.Add("StackTrace", stackTrace);
						}

						var problemDetailsAsString = problemDetails.Serialize();

						await context.Response.WriteAsync(problemDetailsAsString);
					}
				});
			});
		}
	}
}
