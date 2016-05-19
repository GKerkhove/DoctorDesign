using System;
using System.Reflection;
using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.mappers
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class HttpResponseSpriteBodyAttribute : HttpResponseBinaryValueAttribute
	{
		public HttpResponseSpriteBodyAttribute()
		{
			Converter = typeof(DeserializeSprite);
		}
	}
}
