using ToyRobot.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToyRobot.Api.Swagger;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
	/// <summary>
	/// Applies the this filter on swagger documentation generation.
	/// </summary>
	/// <param name="operation"></param>
	/// <param name="context"></param>
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var methodInfo = context.MethodInfo;

		if (methodInfo == null)
		{
			return;
		}

		// then check if there is a method-level 'AllowAnonymous', as this overrides any controller-level 'Authorize'
		var anonControllerScope = methodInfo.DeclaringType?
			.GetCustomAttributes(true)
			.OfType<AllowAnonymousAttribute>();
		var anonMethodScope = methodInfo
			.GetCustomAttributes(true)
			.OfType<AllowAnonymousAttribute>();

		// only add authorization specification information if there is at least one 'Authorize' in the chain and NO method-level 'AllowAnonymous'
		if (!anonMethodScope.Any() && !(anonControllerScope?.Any() ?? false))
		{
			// add generic message if the controller methods don't already specify the response type
			if (!operation.Responses.ContainsKey("401"))
			{
				operation.Responses.Add("401", new OpenApiResponse
				{
					Description = "If Authorization header not present, has no value or no valid jwt bearer token"
				});
			}

			if (!operation.Responses.ContainsKey("403"))
			{
				operation.Responses.Add("403", new OpenApiResponse
				{
					Description = "If user not authorized to perform requested action"
				});
			}

			var jwtAuthScheme = new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = SwaggerStartupExtensions.OPEN_API_SECURITY_SCHEME_NAME_OAUTH2
				}
			};

			operation.Security = new List<OpenApiSecurityRequirement>
			{
				new()
				{
					[ jwtAuthScheme ] = new List<string>()
				}
			};
		}
	}
}
