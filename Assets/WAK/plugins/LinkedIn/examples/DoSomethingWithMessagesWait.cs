using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithMessagesWait: MonoBehaviour
	{
		public behaviors.LinkedInSendMessageBehavior MessageBehavior;

		public string[] SendToIds;

		public bool CopySelf = false;

		public string Subject;

		public string Body;

		IEnumerator Start()
		{
			Configuration.Log("Messages Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			MessageBehavior.ToProfileIds = SendToIds;
			MessageBehavior.CopySelf = CopySelf;
			MessageBehavior.Subject = Subject;
			MessageBehavior.Body = Body;
			yield return StartCoroutine(MessageBehavior.ExecuteAndWait());

			Configuration.Log("Messages call is done with status : " + MessageBehavior.Status, LogSeverity.VERBOSE);

			yield break;
		}
	}
}
