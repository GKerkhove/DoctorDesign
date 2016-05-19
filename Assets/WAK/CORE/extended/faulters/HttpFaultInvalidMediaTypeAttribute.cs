using System;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.faulters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpFaultInvalidMediaTypeAttribute : HttpFaultAttribute
	{
		public override void CheckFaults (hg.ApiWebKit.core.http.HttpOperation operation, hg.ApiWebKit.core.http.HttpResponse response)
		{
			if(response.Request.RequestModelResult.Headers.ContainsKey("Accept") &&
			   response.Headers.ContainsKey("Content-Type"))
			{
				string requestAcceptHeader = ((string)response.Request.RequestModelResult.Headers["Accept"]).ToLower();

				string[] acceptedMediaTypes = requestAcceptHeader.Split(new[] {','},StringSplitOptions.RemoveEmptyEntries);

				string responseContentType = ((string)response.Headers["Content-Type"]).ToLower();

				for(int i=0; i < acceptedMediaTypes.Length; i++)
				{
					acceptedMediaTypes[i] = acceptedMediaTypes[i].Trim();

					if(acceptedMediaTypes[i].Contains("/*"))
						acceptedMediaTypes[i] = acceptedMediaTypes[i].Remove(acceptedMediaTypes[i].IndexOf("/*"), acceptedMediaTypes[i].Length - acceptedMediaTypes[i].IndexOf("/*"));
				
					if(acceptedMediaTypes[i] == "*" || responseContentType.StartsWith(acceptedMediaTypes[i]))
						return;
				}

				operation.Fault("Unexpected media type.  Expected '" + requestAcceptHeader + "' but received '" + responseContentType + "'.");
			}
		}
	}
}
