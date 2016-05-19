using System;
using System.Reflection;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.core.attributes
{
	public abstract class HttpHeaderAttribute : HttpMappedValueAttribute
	{

		/*public HttpHeaderAttribute():base(MappingDirection.ALL, null)
		{
			Value = null;
		}

		public HttpHeaderAttribute(string header):base(MappingDirection.ALL, header)
		{
			Value = null;
		}

		protected HttpHeaderAttribute(string header, string value):base(MappingDirection.ALL, header)
		{
			Value = value;
		}*/

		protected HttpHeaderAttribute(MappingDirection direction, string header, string value):base(direction, header)
		{
			Value = value;
		}
		
		protected HttpHeaderAttribute(MappingDirection direction, string header):base(direction, header)
		{
			Value = null;
		}

		public override void OnRequestResolveModel (string name, object value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			if(!string.IsNullOrEmpty(name))
			{
				model.AddHttpHeader(
					name,
					(@value == null) ? "" : @value.ToString()
				);
			}
			else
				operation.Log ("(HttpHeaderAttribute)(OnRequestResolveModel) Header failed to add because the name is empty!", LogSeverity.WARNING);
		}

		public override object OnResponseResolveValue (string name, HttpOperation operation, FieldInfo fi, HttpResponse response)
		{
			//headers should be case sensitive as set in provider code
			if(!string.IsNullOrEmpty(name))
			{
				if(response.Headers.ContainsKey(name))
					return response.Headers[name];
				else
				{
					operation.Log("(HttpHeaderAttribute)(OnResponseResolveValue) Key '" + name + "' is not found in response headers for '" + fi.Name + "'.", LogSeverity.WARNING);
					return null;
				}
			}
			else
			{
				operation.Log ("(HttpHeaderAttribute)(OnResponseResolveValue) Header failed to fetch because the name is empty!", LogSeverity.WARNING);
				return null;
			}
		}
	}
}
