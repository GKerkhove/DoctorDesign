using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInSendMessageBehavior : ApiBehavior<LinkedInSendMessageBehavior>
	{
		public string[] ToProfileIds = null;
		public bool CopySelf = false;
		public string Subject = null;
		public string Body = null;

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.SendMessage(
				ToProfileIds,
				CopySelf,
				Subject,
				Body,
				new HttpCallbacks<operations.SendMessage> {
					done = new Action<operations.SendMessage, HttpResponse>
						((operation, response) => 
						 { 
							Status = ApiBehaviorStatus.SUCCESS;
						}),
					fail = new Action<operations.SendMessage, HttpResponse>
						((operation, response) => 
						 { 
							Status = ApiBehaviorStatus.FAILURE;
						}),
					always = new Action<operations.SendMessage, HttpResponse>
						((operation, response) => 
						 { 
							OnCompletion(operation, response, LinkedInApiMonitor.Instance);
						})
				}
			);
		}
	}
}
