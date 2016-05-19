using System;
using System.Reflection;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.converters;


namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpRequestSoapBodyAttribute : HttpMappedValueAttribute
	{
		public HttpRequestSoapBodyAttribute():base(MappingDirection.REQUEST,null) 
		{ 
			Converter = typeof(SerializeSoap);
		}

		public override void OnRequestResolveModel (string name, object value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			model.SetStringBody( 
            	(@value == null) ? "<xml></xml>" : (string)@value
            );
		}
	}
}
