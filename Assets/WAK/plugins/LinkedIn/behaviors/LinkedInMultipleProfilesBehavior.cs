using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInMultipleProfilesBehavior : ApiBehavior<LinkedInMyProfileBehavior>
	{
		[NullifyOnQueryAttribute]
		public models.PersonList Profiles;

		public string[] ProfileIdentifiers;

		public bool SecureUrls;

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.GetMultipleProfiles(
				new HttpCallbacks<operations.GetProfile> {
				done = new Action<operations.GetProfile, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.SUCCESS;
					}),
				fail = new Action<operations.GetProfile, HttpResponse>
					((operation, response) => 
					 { 
						Status = ApiBehaviorStatus.FAILURE;
					}),
				always = new Action<operations.GetProfile, HttpResponse>
					((operation, response) => 
					 { 
						Profiles = operation.profiles;
						
						OnCompletion(operation, response, LinkedInApiMonitor.Instance);
					})
				},
				SecureUrls,
				ProfileIdentifiers
			);
		}
	}
}
