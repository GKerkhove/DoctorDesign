using UnityEngine;
using System;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public abstract class HttpAuthorizationAttribute : HttpMappedValueAttribute
	{
		public HttpAuthorizationAttribute(PLACEMENT placement, string name): base(MappingDirection.REQUEST, name)
		{
			Placement = placement;
		}
		
		public PLACEMENT Placement;
		
		public enum PLACEMENT
		{
			NONE,
			HEADER,
			QUERY_STRING
		}
		
		public override void OnRequestResolveModel (string name, object value, ref HttpRequestModel model, hg.ApiWebKit.core.http.HttpOperation operation, System.Reflection.FieldInfo fi)
		{
			
		}
	}
}
