using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithIdInvitationsWait: MonoBehaviour
	{
		public behaviors.LinkedInSendInvitationByIdBehavior IdInvitationsBehavior;

		public models.InviteById Invitee;

		public string Subject;

		public string Body;

		IEnumerator Start()
		{
			Configuration.Log("IdInvitations Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			IdInvitationsBehavior.Invitee = Invitee;
			IdInvitationsBehavior.Subject = Subject;
			IdInvitationsBehavior.Body = Body;
			yield return StartCoroutine(IdInvitationsBehavior.ExecuteAndWait());

			Configuration.Log("IdInvitations call is done with status : " + IdInvitationsBehavior.Status, LogSeverity.VERBOSE);

			yield break;
		}
	}
}
