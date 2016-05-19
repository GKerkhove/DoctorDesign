using System;
using System.Reflection;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;


namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class HttpResponseBinaryBodyAttribute : HttpResponseBinaryValueAttribute
	{

	}
}
