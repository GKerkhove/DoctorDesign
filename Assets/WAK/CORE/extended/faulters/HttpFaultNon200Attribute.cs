using System;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.faulters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpFaultNon200Attribute : HttpFaultAttribute
	{
		public override void CheckFaults (HttpOperation operation, HttpResponse response)
		{
			if(response.StatusCode != HttpStatusCode.OK)
				operation.Fault("Response status code is not 200.");
		}
	}
}
