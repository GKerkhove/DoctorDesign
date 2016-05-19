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
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class RefreshOAuthDeviceAccessToken: HttpOperation
	{
		[HttpFormField]
		public string client_id;

		[HttpFormField]
		public string client_secret;

		[HttpFormField]
		public string refresh_token;

		[HttpFormField(Value="refresh_token")]
		public string grant_type;
		
		[HttpResponseJsonBody]
		public models.OAuthFault OAuthFault;

		[HttpResponseJsonBody]
		public models.OAuthDeviceAccessToken Token;
	}
}