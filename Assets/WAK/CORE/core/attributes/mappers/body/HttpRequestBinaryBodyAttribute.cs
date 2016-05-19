using System;
using System.Reflection;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;


namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class HttpRequestBinaryBodyAttribute : HttpRequestBinaryValueAttribute
	{
		public override void OnRequestResolveModel (string name, object value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			model.SetBinaryBody( 
				(@value == null) ? null : (byte[])@value
			);
		}
	}
}
