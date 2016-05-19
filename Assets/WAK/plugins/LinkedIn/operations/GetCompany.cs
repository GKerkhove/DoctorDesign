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
	[HttpPath("linkedin","/companies{$id}{$fieldSelectors}")]
	public class GetCompany : LinkedInOperationSecureGet
	{
		public GetCompany ById(string companyId)
		{
			id = "/" + companyId;
			return this;
		}
		
		public GetCompany ByDomain(string companyDomain)
		{
			emailDomain = companyDomain;
			return this;
		}

		/* request */

		[HttpUriSegment(VariableValue = "linkedin.company.fieldSelectors")]
		public string fieldSelectors;

		[HttpUriSegmentAttribute]
		public string id;

		[HttpQueryString("email-domain")]
		public string emailDomain;

		/* response */

		[HttpResponseJsonBody]
		public models.Companies companies;

		[HttpResponseJsonBody]
		public models.Company company;

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

