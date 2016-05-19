using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithProfileByIdWait : MonoBehaviour
	{
		public behaviors.LinkedInProfileByIdBehavior ProfileBehavior;

		public string ProfileId;

		[SerializeField]
		private models.PersonList _profiles;

		IEnumerator Start()
		{
			Configuration.Log("IdProfile Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));
			
			ProfileBehavior.ProfileId = ProfileId;
			yield return StartCoroutine(ProfileBehavior.ExecuteAndWait());
			_profiles = ProfileBehavior.Profiles;

			Configuration.Log("IdProfile call is done with status : " + ProfileBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("IdProfile " + _profiles, LogSeverity.VERBOSE); 
			
			yield break;
		}
	}
}
