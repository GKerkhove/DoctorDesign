using System;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.core.attributes
{
	public abstract class HttpFaultAttribute : Attribute
	{
		public virtual void CheckFaults(HttpOperation operation, HttpResponse response)
		{

		}
	}
}
