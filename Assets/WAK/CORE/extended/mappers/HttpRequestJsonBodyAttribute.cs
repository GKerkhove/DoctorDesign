using System;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using System.Reflection;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpRequestJsonBodyAttribute : HttpMappedValueAttribute
	{
#if UNITY_5_3
		public HttpRequestJsonBodyAttribute(bool useUnitySerializer = false):base(MappingDirection.REQUEST,null) 
		{ 
			Converter = useUnitySerializer ? typeof(SerializeUnityJsonUtility) : typeof(SerializeLitJson);
		}
#else
		public HttpRequestJsonBodyAttribute(bool useUnitySerializer = false):base(MappingDirection.REQUEST,null) 
		{ 
			Converter = typeof(SerializeLitJson);
		}
#endif

		public override void OnRequestResolveModel (string name, object value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			model.SetStringBody( 
            	(@value == null) ? "{}" : (string)@value
            );
		}
	}
}
