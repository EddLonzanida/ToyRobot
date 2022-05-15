using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ToyRobot.Api.Middlewares.BaseClasses
{
	public abstract class MiddlewareBase
	{
		protected readonly RequestDelegate Next;
		protected readonly string Env;

		[DebuggerStepThrough]
		protected MiddlewareBase(RequestDelegate next, IHostEnvironment env)
		{
			Next = next;
			Env = env.EnvironmentName;
		}

		public abstract Task Invoke(HttpContext context);
	}
}
