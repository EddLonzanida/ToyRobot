using Eml.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobot.Api.Extensions
{
	public static class UtilityExtensions
	{
		public static async Task<string> GetRequestBodyAsString(this HttpContext httpContext)
		{
			const int bufferSize1Kb = 1024;

			var body = string.Empty;

			httpContext.Request.EnableBuffering();

			if (httpContext.Request.ContentLength == null
				|| !(httpContext.Request.ContentLength > 0)
				|| !(httpContext.Request.Body.CanSeek)
			)
			{
				return body;
			}

			httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

			using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, bufferSize1Kb, true))
			{
				body = await reader.ReadToEndAsync();
			}

			httpContext.Request.Body.Position = 0;

			return body;
		}

		public static async Task<object?> GetRequestBody(this HttpContext httpContext)
		{
			var body = await httpContext.GetRequestBodyAsString();

			try
			{
				var result = JsonConvert.DeserializeObject(body);

				return result;
			}
			catch (Exception)
			{
				return body;
			}
		}

		/// <summary>
		/// Retrieve release version from the assembly.
		/// <para>Assembly Version updates are done during CI/CD publish. See Dockerfile.</para>
		/// </summary>
		/// <returns></returns>
		public static string GetAssemblyVersion()
		{
			var assembly = typeof(Startup).Assembly;
			var customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			var customAttributesList = customAttributes.ToList();

			if (!customAttributesList.Any()) return "n/a";

			var assemblyCopyright = customAttributesList.First() as AssemblyCopyrightAttribute;

			// Assembly Version updates are done during CI/CD publish.
			// Docker file command:
			// RUN dotnet publish "ToyRobot.Api.csproj" -c Release -o /app/publish /p:Copyright=$RELEASE_VERSION
			// Copyright property is used because as of the moment, Version property is restricted to major.minor.build.revision format.
			var copyRight = assemblyCopyright?.Copyright == null ? string.Empty : assemblyCopyright.Copyright;
			var assemblyVersion = copyRight;

			return assemblyVersion;
		}

		/// <summary>
		/// <para>Uses type name as the configuration key. Only works on classes that ends with "Config".</para>
		/// <para>For other complex types, use <see cref="GetConfigValue{T}(IConfiguration,string)"/></para>
		/// </summary>
		public static T GetConfigValue<T>(this IConfiguration configuration)
			where T : new()
		{
			var item = new T();
			var key = typeof(T).Name.TrimRight("Config");

			configuration.GetSection(key).Bind(item);

			return item;
		}

		/// <summary>
		/// <para>Extract the configuration with the given <paramref name="key"/> and convert its value to <typeparamref name="T"/>.</para>
		/// <para>For native types, use <see cref="ConfigurationBinder.GetValue{T}(IConfiguration,string)"/></para>
		/// </summary>
		public static T GetConfigValue<T>(this IConfiguration configuration, string key)
			where T : new()
		{
			var item = new T();

			configuration.GetSection(key).Bind(item);

			return item;
		}
	}
}
