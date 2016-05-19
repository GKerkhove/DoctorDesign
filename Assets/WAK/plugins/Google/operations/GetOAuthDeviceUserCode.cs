using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.google.operations
{ 
	[HttpPath(null,"https://accounts.google.com/o/oauth2/device/code")]
	[HttpPOST]
	[HttpFaultNon2XX]
	[HttpContentType("application/x-www-form-urlencoded")]
	[HttpFaultWhen("OAuthFault.error", HttpFaultWhenCondition.IsNot, null)]
	[HttpFaultWhen("Code.user_code", HttpFaultWhenCondition.Is, null)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class GetOAuthDeviceUserCode: HttpOperation
	{
		[HttpFormField]
		public string client_id;

		[HttpFormField]
		public string scope;

		[HttpResponseJsonBody]
		public models.OAuthDeviceUserCode Code;
		
		[HttpResponseJsonBody]
		public models.OAuthFault OAuthFault;
	}
}

