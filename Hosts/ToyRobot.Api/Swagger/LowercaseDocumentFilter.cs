using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ToyRobot.Api.Swagger
{
	/// <summary>
	/// Lowercase all urls except the parameters.
	/// </summary>
	public class LowercaseDocumentFilter : IDocumentFilter
	{
		private static string LowercaseEverythingButParameters(string key)
		{
			return string.Join("/", key.Split("/").Select(x => x.Contains("{") ? x : x.ToLower()));
		}

		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			var paths = new OpenApiPaths();

			foreach (var (k, value) in swaggerDoc.Paths)
			{
				var key = LowercaseEverythingButParameters(k);

				paths[key] = value;
			}

			swaggerDoc.Paths = paths;
		}
	}
}
