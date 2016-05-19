using System;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using System.Reflection;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpRequestTextBodyAttribute : HttpMappedValueAttribute
	{
		public HttpRequestTextBodyAttribute():base(MappingDirection.REQUEST,null) 
		{ 
			
		}

		public override void OnRequestResolveModel (string name, object value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			model.SetStringBody( 
            	(@value == null) ? "" : (string)@value
            );
		}
	}
}
