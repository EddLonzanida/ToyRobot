using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using ToyRobot.Api.Middlewares.BaseClasses;
using ToyRobot.Infrastructure;

namespace ToyRobot.Api.Middlewares
{
	public class XApiMiddleware : MiddlewareBase
	{
		[DebuggerStepThrough]
		public XApiMiddleware(RequestDelegate next, IHostEnvironment env)
			: base(next, env)
		{
		}

		public override async Task Invoke(HttpContext context)
		{
			var releaseIdVersion = AppConstants.ReleaseVersion;

			context.Response.Headers.Add("X-Api-Version", releaseIdVersion);
			context.Response.Headers.Add("X-Api-Environment", Env);

			await Next.Invoke(context);
		}
	}
}
