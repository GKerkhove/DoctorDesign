using System;

namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpProviderAttribute : Attribute
	{
		public Type ProviderType;

		/// <summary>
		/// Define the HTTP provider type that will transport your data (eg: Unity WWW, UniWeb, BestHTTP, DotNet WebRequest).  HTTP providers inherit and implement HttpAbstractProvider.
		/// </summary>
		public HttpProviderAttribute(Type providerType)
		{
			ProviderType = providerType;
		}
	}
}
