using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using ToyRobot.Api.Middlewares.BaseClasses;
using ToyRobot.Infrastructure.Extensions;

namespace ToyRobot.Api.Middlewares
{
	/// <summary>
	/// <para>Send log of aspnet-requestDto-posted-body in non-production only.</para>
	/// <para>Needed for ${aspnet-requestDto-posted-body} in appsettings.</para>
	/// <para>Must be before app.UseEndpoints.</para>
	/// </summary>
	public class NLogEnableRequestBufferingMiddleware : MiddlewareBase
	{
		[DebuggerStepThrough]
		public NLogEnableRequestBufferingMiddleware(RequestDelegate next, IHostEnvironment env)
			: base(next, env)
		{
		}

		public override async Task Invoke(HttpContext context)
		{
			//disable if in prd environment, request body might expose PII
			if (!Env.IsProductionEnvironment()) context.Request.EnableBuffering();

			await Next.Invoke(context);
		}
	}
}
