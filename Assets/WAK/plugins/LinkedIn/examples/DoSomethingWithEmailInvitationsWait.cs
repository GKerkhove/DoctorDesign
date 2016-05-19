using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithEmailInvitationsWait: MonoBehaviour
	{
		public behaviors.LinkedInSendInvitationByEmailBehavior EmailInvitationsBehavior;

		public models.InviteByEmail Invitee;

		public string Subject;

		public string Body;

		IEnumerator Start()
		{
			Configuration.Log("EmailInvitations Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			EmailInvitationsBehavior.Invitee = Invitee;
			EmailInvitationsBehavior.Subject = Subject;
			EmailInvitationsBehavior.Body = Body;
			yield return StartCoroutine(EmailInvitationsBehavior.ExecuteAndWait());

			Configuration.Log("EmailInvitations call is done with status : " + EmailInvitationsBehavior.Status, LogSeverity.VERBOSE);

			yield break;
		}
	}
}
