using System;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpContentTypeAttribute : HttpHeaderAttribute
	{
		public HttpContentTypeAttribute(string contentType): base(MappingDirection.REQUEST,"Content-Type",contentType)
		{
		}
	}
}
