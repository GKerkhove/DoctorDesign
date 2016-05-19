using System;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using System.Reflection;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpResponseSoapBodyAttribute : HttpResponseTextBodyAttribute
	{
		public HttpResponseSoapBodyAttribute():base() 
		{ 
			Converter = typeof(DeserializeSoap);
		}
	}
}
