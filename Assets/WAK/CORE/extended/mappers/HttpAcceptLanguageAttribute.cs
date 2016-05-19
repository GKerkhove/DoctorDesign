using System;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpAcceptLanguageAttribute : HttpHeaderAttribute
	{
		public HttpAcceptLanguageAttribute(string language): base(MappingDirection.REQUEST,"Accept-Language",language)
		{
		}
	}
}
