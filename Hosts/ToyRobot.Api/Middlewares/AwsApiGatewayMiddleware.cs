using ToyRobot.Api.Middlewares.BaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using ToyRobot.Infrastructure.Extensions;

namespace ToyRobot.Api.Middlewares;

public class AwsApiGatewayMiddleware : MiddlewareBase
{
	public AwsApiGatewayMiddleware(RequestDelegate next, IHostEnvironment env)
		: base(next, env)
	{
	}

	public override async Task Invoke(HttpContext context)
	{
		if (!Env.IsLocalEnvironment())
		{
			context.Request.PathBase = new PathString($"/{Env}");
		}

		await Next.Invoke(context);
	}
}
