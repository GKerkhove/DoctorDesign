using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInCompanyByIdBehavior : ApiBehavior<LinkedInCompanyByIdBehavior>
	{
		[NullifyOnQueryAttribute]
		public models.Company Company;

		public string CompanyId = "";

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.GetCompanyById(
				CompanyId,
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
						Company = operation.company;
						
						OnCompletion(operation, response, LinkedInApiMonitor.Instance);
					})
				}
			);
		}
	}
}
