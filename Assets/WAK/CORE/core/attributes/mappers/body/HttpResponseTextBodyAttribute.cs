using System;
using System.Reflection;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;


namespace hg.ApiWebKit.core.attributes
{
	public class HttpResponseTextBodyAttribute : HttpMappedValueAttribute
	{
		public HttpResponseTextBodyAttribute():base(MappingDirection.RESPONSE,null) 
		{ 
			
		}

		public override object OnResponseResolveValue (string name, HttpOperation operation, FieldInfo fi, HttpResponse response)
		{
			return response.Text;
		}
	}
}
