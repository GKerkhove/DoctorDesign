using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInCompanyByDomainBehavior : ApiBehavior<LinkedInCompanyByDomainBehavior>
	{
		[NullifyOnQueryAttribute]
		public models.Companies Companies;

		public string CompanyDomain = "";

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.GetCompanyByDomain(
				CompanyDomain,
				new HttpCallbacks<operations.GetCompany> {
				done = new Action<operations.GetCompany, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.SUCCESS;
					}),
					fail = new Action<operations.GetCompany, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.FAILURE;
					}),
					always = new Action<operations.GetCompany, HttpResponse>
					((operation, response) => 
					 { 
						Companies = operation.companies;
						
						OnCompletion(operation, response, LinkedInApiMonitor.Instance);
					})
				}
			);
		}
	}
}
