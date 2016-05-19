using System;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using System.Reflection;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpResponseAssemblyBodyAttribute : HttpResponseBinaryValueAttribute
	{
		public HttpResponseAssemblyBodyAttribute():base() 
		{ 
			Converter = typeof(DeserializeAssembly);
		}
	}
}
