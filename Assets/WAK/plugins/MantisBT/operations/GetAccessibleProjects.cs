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
	public class GetAccessibleProjects : HttpOperation
	{
		[HttpRequestSoapBody]
		public models.ProjectDataRequest Request;

		[HttpResponseSoapBody(Converters = new Type[] { typeof(converters.ResponseStripper) })]
		public models.ProjectDataResponse Response;
		
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