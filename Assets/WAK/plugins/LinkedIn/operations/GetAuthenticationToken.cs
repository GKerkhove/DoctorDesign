using UnityEngine;

using System;
using System.Text;
using System.Collections;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;
using hg.ApiWebKit.converters;

namespace hg.ApiWebKit.apis.linkedin.operations
{ 
	[HttpPOST]
	[HttpPath(null,"https://www.linkedin.com/uas/oauth2/accessToken")]
	[HttpFaultNon200]
	public class GetAuthenticationToken : LinkedInOperation
	{
		public GetAuthenticationToken SetParameters(string authorizationCode, string apiKey, string secretKey, string redirectUrl)
		{
			code = authorizationCode;
			client_id = apiKey;
			client_secret = secretKey;
			redirect_uri = redirectUrl;

			return this;
		}

		/* request */

		[HttpQueryString(Value = "authorization_code")]
		public string grant_type;

		[HttpQueryString]
		public string code;

		[HttpQueryString]
		public string client_id;

		[HttpQueryString]
		public string client_secret;

		[HttpQueryString]
		public string redirect_uri;

		/* response */

		[HttpResponseJsonBody]
		public models.AuthenticationToken token;
	}
}

