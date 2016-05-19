using UnityEngine;

using System;
using System.Text;
using System.Collections;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.google.operations
{ 
	[HttpPath(null,"https://www.googleapis.com/oauth2/v1/tokeninfo")]
	[HttpGET]
	[HttpFaultNon200]
	[HttpFaultWhen("OAuthFault.error", HttpFaultWhenCondition.IsNot, null)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class GetOAuthTokenInfo: HttpOperation
	{
		[HttpQueryString]
		public string access_token;
		
		[HttpResponseJsonBody]
		public models.OAuthTokenInfo TokenInformation;
		
		[HttpResponseJsonBody]
		public models.OAuthFault OAuthFault;	
	}
}

