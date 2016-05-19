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
	[HttpPath(null,"https://accounts.google.com/o/oauth2/token")]
	[HttpPOST]
	[HttpFaultNon2XX]
	[HttpContentType("application/x-www-form-urlencoded")]
	[HttpFaultWhen("OAuthFault.error", HttpFaultWhenCondition.IsNot, null)]
	[HttpTimeout(10)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class GetOAuthJwtAccessToken: HttpOperation
	{
		[HttpFormField(Value="urn:ietf:params:oauth:grant-type:jwt-bearer")]
		public string grant_type;

		[HttpFormField]
		public string assertion;

		[HttpResponseJsonBody]
		public models.OAuthFault OAuthFault;
		
		[HttpResponseJsonBody]
		public models.OAuthJwtAccessToken Token;

		/*protected override HttpRequest ToRequest (params string[] parameters)
		{
			assertion = GoogleJsonWebToken.Encode(null,null);
			
			return base.ToRequest (parameters);
		}*/
	}
}