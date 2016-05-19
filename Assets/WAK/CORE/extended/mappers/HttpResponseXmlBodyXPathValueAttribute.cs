using System;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class HttpResponseXmlBodyXPathValueAttribute : HttpResponseTextBodyAttribute
	{
		public HttpResponseXmlBodyXPathValueAttribute(string xPathExpression): base()
		{
			Converter = typeof(XmlXPathValue);
			XPathExpression = xPathExpression;
		}
		
		public string XPathExpression = "";
	}
}
