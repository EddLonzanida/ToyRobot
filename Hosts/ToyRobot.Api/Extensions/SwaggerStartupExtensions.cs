using ToyRobot.Api.Swagger;
using ToyRobot.Infrastructure;
using ToyRobot.Infrastructure.Configurations;
using Eml.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ToyRobot.Api.Extensions
{
	public static class SwaggerStartupExtensions
	{
		private const string SwaggerDocVersion = "v1";

		private const string LaunchUrl = "docs";

		public const string OPEN_API_SECURITY_SCHEME_NAME_OAUTH2 = "OAuth2";

		/// <summary>
		/// Set UI title, version, contact and description.
		/// <para>Display the actual c# method for the url. Lowercase all urls except the parameters.</para>
		/// <para>Describe all parameters, regardless of how they appear in code, in camelCase.</para>
		/// <para>Add Jwt Bearer Security Definition.</para>
		/// </summary>
		public static IServiceCollection ConfigureSwaggerService(this IServiceCollection services, IHostEnvironment env, string assemblyVersion, string urlAuth0JwtAuthority, string auth0JwtConfigAudience)
		{
			var releaseIdVersion = $"{env.EnvironmentName} release: {assemblyVersion}";

			return services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(SwaggerDocVersion,
					new OpenApiInfo
					{
						Title = AppConstants.ApplicationName,
						Version = releaseIdVersion,
						Contact = new OpenApiContact { Name = "Eddie Lonzanida", Email = "EddieLonzanida@hotmail.com" },
						Description = "SOA Solution using .Net6. Featuring Angular13, Etags, RateLimits, IoC/DI, EFCore, DataMigrations, SwaggerUI, XUnit , DataRepository pattern, NLog, HealthChecks and more.."
					});

				c.OperationFilter<SwashbuckleSummaryOperationFilter>();
				c.OperationFilter<SecurityRequirementsOperationFilter>();
				c.DocumentFilter<LowercaseDocumentFilter>();
				c.DescribeAllParametersInCamelCase();
				//c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

				var xmlPath = ".xml".GetNewPathFromAssemblyOf<Program>();
				var scopes = new[] { "openid", "profile", "api-scope" };
				var scopesAsDictionary = scopes.ToDictionary(x => x, x => string.Empty);

				c.IncludeXmlComments(xmlPath);
				c.AddSecurityDefinition(OPEN_API_SECURITY_SCHEME_NAME_OAUTH2, new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "<h5>Login using your Email credentials.</h5>",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.OAuth2,
					Flows = new OpenApiOAuthFlows
					{
						Implicit = new OpenApiOAuthFlow
						{
							AuthorizationUrl = new Uri($"{urlAuth0JwtAuthority}/authorize?audience={auth0JwtConfigAudience}"),
							Scopes = scopesAsDictionary
						}
					}
				});
			})
			.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()
		}

		/// <summary>
		/// Set '/docs' endpoint and collapse all UI panels by default.
		/// </summary>
		public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, Auth0JwtConfig? auth0JwtConfig)
		{
			return app.UseSwagger(c =>
				{
					c.RouteTemplate = $"{LaunchUrl}/swagger/{{documentName}}/swagger.json";
				})
				.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint($"swagger/{SwaggerDocVersion}/swagger.json", AppConstants.ApplicationName);
					c.RoutePrefix = LaunchUrl;
					c.EnableFilter();
					c.DocExpansion(DocExpansion.None);
					c.DocumentTitle = AppConstants.ApplicationName;

					var clientId = auth0JwtConfig?.ClientId ?? string.Empty;

					c.OAuthClientId(clientId); // pre-fill this field in swagger.
				});
		}
	}
}
