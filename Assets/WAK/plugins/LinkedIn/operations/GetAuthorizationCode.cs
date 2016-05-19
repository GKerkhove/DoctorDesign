using UnityEngine;

using System;
using System.Text;
using System.Collections;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.linkedin.operations
{ 
	[HttpGET]
	[HttpPath(null,"https://www.linkedin.com/uas/oauth2/authorization")]
	[HttpFaultNon200]
	public class GetAuthorizationCode : LinkedInOperation
	{
		public GetAuthorizationCode SetParameters(string apiKey, string redirectUrl)
		{
			client_id = apiKey;
			redirect_uri = redirectUrl;

			return this;
		}

		/* request */

		[HttpQueryString(Value = "code")]
		public string response_type;

		[HttpQueryString]
		public string client_id;

		[HttpQueryString(Value = "rw_nus r_contactinfo w_messages r_emailaddress rw_company_admin rw_groups r_network r_fullprofile", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string scope;

		[HttpQueryString(Value = "uniqueStateStringGoesHere")]
		public string state;

		[HttpQueryString]
		public string redirect_uri;

		/* response */
	}
}

