using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;
//TODO
//https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id=77grnetf3wsgvt&redirect_uri=https%3A%2F%2Fwww.example.com%2Fauth%2Flinkedin&state=987654321&scope=r_basicprofile
//Deze linkt pakt een acces token ofzo?
//Verder moet er een oauth 2 site gemaakt wroden, jimi all yours baby?
namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInProfileByUrlBehavior : ApiBehavior<LinkedInProfileByUrlBehavior>
	{
		[NullifyOnQueryAttribute]
		public models.PersonList Profiles;

		public string ProfileUrl;

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
				ProfileUrl
			);
		}
	}
}
