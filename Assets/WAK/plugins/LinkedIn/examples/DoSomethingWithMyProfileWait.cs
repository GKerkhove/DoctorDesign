using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithMyProfileWait : MonoBehaviour
	{
		public behaviors.LinkedInMyProfileBehavior ProfileBehavior;

		[SerializeField]
		private models.PersonList _profiles;

		IEnumerator Start()
		{
			Configuration.Log("MyProfile Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			yield return StartCoroutine(ProfileBehavior.ExecuteAndWait());
			_profiles = ProfileBehavior.Profiles;

			Configuration.Log("Retrieved my profile with status : " + ProfileBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("MyProfile " + _profiles, LogSeverity.VERBOSE); 

			yield break;
		}
	}
}
