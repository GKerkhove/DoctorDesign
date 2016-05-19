using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInJobSearchByCompanyNameBehavior : ApiBehavior<LinkedInJobSearchByCompanyNameBehavior>
	{
		[NullifyOnQueryAttribute]
		public models.Jobs Jobs;

		public string CompanyName = "";

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.GetJobByCompany(
				CompanyName,
				new HttpCallbacks<operations.GetJobs> {
				done = new Action<operations.GetJobs, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.SUCCESS;
					}),
					fail = new Action<operations.GetJobs, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.FAILURE;
					}),
						always = new Action<operations.GetJobs, HttpResponse>
					((operation, response) => 
					 { 
						Jobs = operation.jobs;
						
						OnCompletion(operation, response, LinkedInApiMonitor.Instance);
					})
				}
			);
		}
	}
}
