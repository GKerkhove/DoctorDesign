using System;

namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpTimeoutAttribute : Attribute
	{
		public float Timeout;

		/// <summary>
		/// Define API operation timeout in seconds.
		/// </summary>
		public HttpTimeoutAttribute(float timeout)
		{
			Timeout = timeout;
		}
	}
}
