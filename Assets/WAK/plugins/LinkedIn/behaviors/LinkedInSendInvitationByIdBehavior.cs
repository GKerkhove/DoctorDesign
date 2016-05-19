using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.linkedin.behaviors
{
	public class LinkedInSendInvitationByIdBehavior : ApiBehavior<LinkedInSendInvitationByIdBehavior>
	{
		public models.InviteById Invitee = null;
		public string Subject = null;
		public string Body = null;

		public override void ExecutableCode()
		{
			LinkedInProxy.Instance.SendInvitationById(
				Invitee,
				Subject,
				Body,
				new HttpCallbacks<operations.SendInvitation> {
					done = new Action<operations.SendInvitation, HttpResponse>
						((operation, response) => 
						 { 
							Status = ApiBehaviorStatus.SUCCESS;
						}),
					fail = new Action<operations.SendInvitation, HttpResponse>
						((operation, response) => 
						 { 
							Status = ApiBehaviorStatus.FAILURE;
						}),
					always = new Action<operations.SendInvitation, HttpResponse>
						((operation, response) => 
						 { 
							OnCompletion(operation, response, LinkedInApiMonitor.Instance);
						})
				}
			);
		}
	}
}
