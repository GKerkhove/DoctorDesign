using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Reflection;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;

namespace hg.ApiWebKit.apis.mantisbt.operations
{
	[HttpPOST]
	[HttpTimeout(10f)]
	[HttpPath("mantisbt","/api/soap/mantisconnect.php")]

	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpBestHttpClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpUniWebClient))]

	[HttpContentType("text/xml")]
	public class GetVersionInformation : HttpOperation
	{
		[HttpRequestSoapBody]
		public models.VersionRequest Request;

		[HttpResponseSoapBody]
		public models.VersionResponse Response;
		
		protected override HttpRequest ToRequest (params string[] parameters)
		{
			return base.ToRequest (parameters);
		}
		
		protected override void OnRequestComplete (HttpResponse response)
		{
			base.OnRequestComplete (response);
			
			//Debug.Log(this.ToString());
		}
	}
}