using UnityEngine;
using System;
using hg.ApiWebKit.faulters;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.operations
{
	[HttpFaultNon2XX]
	[HttpContentType("application/json")]
	[HttpAccept("application/json")]
	[HttpAcceptLanguage("en-US")]
	[HttpFaultWhen("fault.errorCode", HttpFaultWhenCondition.IsNot, 0)]
	//[HttpRequestHeader("x-li-format","json")] // BUG: This does not work per documentation.
	public abstract class LinkedInOperationSecure : LinkedInOperation
	{
		[HttpQueryString(VariableValue = "linkedin.accesstoken")]
		public string oauth2_access_token;

		[HttpQueryString(VariableValue = "linkedin.dataformat")]
		public string format;

		[HttpResponseJsonBody]
		public models.Fault fault;
	}
}

