using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToyRobot.Api.Swagger
{
	/// <summary>
	/// Display the actual c# method for the url.
	/// </summary>
	public class SwashbuckleSummaryOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

			var header = controllerActionDescriptor.ActionName.Split("_");

			if (header.Length > 1)
			{
				//get underscore separated header 
				var headerOnly = header.ToList().GetRange(1, header.Length - 1).ToArray();

				operation.Summary = string.Join("_", headerOnly);
			}
			else operation.Summary = header.First(); //if headers are not underscore separated

			if (operation.Parameters == null) return;

			var parameters = operation.Parameters.Select(p => p.Name);

			operation.Summary = $"{operation.Summary}({string.Join(", ", parameters)})";
		}
	}
}
