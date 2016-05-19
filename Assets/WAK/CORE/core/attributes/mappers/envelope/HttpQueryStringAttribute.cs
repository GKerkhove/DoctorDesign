using System;
using System.Reflection;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpQueryStringAttribute : HttpMappedValueAttribute
	{
		public bool IgnoreWhenValueIsEmpty = true;

		public HttpQueryStringAttribute():base(MappingDirection.REQUEST,null) { } 

		public HttpQueryStringAttribute(string field):base(MappingDirection.REQUEST,field) { }

		public override void OnRequestResolveModel (string name, object @value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			if(@value != null && @value.GetType().IsArray)
				resolveArray(name,@value,ref model,operation,fi);
			else
				resolve(name,@value,ref model,operation,fi);
		}

		void resolveArray(string name, object @value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			if(!string.IsNullOrEmpty(name))
			{
				foreach(object item in (Array)@value) 
				{
					if((!string.IsNullOrEmpty(item.ToString())) ||
			 			(string.IsNullOrEmpty(item.ToString()) && !IgnoreWhenValueIsEmpty ))
			 		{
						model.AddQueryString(
							name + "[]", 
							(item == null) ? "" : item.ToString()
						);
					}
					else
					{
						operation.Log ("(HttpQueryStringAttribute)(OnRequestResolveModel) Query string part failed to add because the value is empty!", LogSeverity.WARNING);
					}
				}
			}
			else
			{
				operation.Log ("(HttpQueryStringAttribute)(OnRequestResolveModel) Query string part failed to add because the name is empty!", LogSeverity.WARNING);
			}
		}

		void resolve(string name, object @value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			@value = System.Convert.ChangeType(@value,typeof(string)); //TODO : validate support of typed fields

			if(!string.IsNullOrEmpty(name) && 
			   	((!string.IsNullOrEmpty((string)@value)) ||
			 	 (string.IsNullOrEmpty((string)@value) && !IgnoreWhenValueIsEmpty ))
			   )
			{
				model.AddQueryString(
					name, 
					(@value == null) ? "" : @value.ToString()
				);
			}
			else
			{
				operation.Log ("(HttpQueryStringAttribute)(OnRequestResolveModel) Query string part failed to add because the name is empty!", LogSeverity.WARNING);
			}
		}
	}
}
