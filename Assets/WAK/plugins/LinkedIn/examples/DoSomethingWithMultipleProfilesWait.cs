using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithMultipleProfilesWait : MonoBehaviour
	{
		public behaviors.LinkedInMultipleProfilesBehavior ProfileBehavior;

		public string[] ProfileSelectors;

		[SerializeField]
		private models.PersonList _profiles;

		IEnumerator Start()
		{
			Configuration.Log("MultProfiles Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			ProfileBehavior.ProfileIdentifiers = ProfileSelectors;
			yield return StartCoroutine(ProfileBehavior.ExecuteAndWait());
			_profiles = ProfileBehavior.Profiles;

			Configuration.Log("MultProfiles call is done with status : " + ProfileBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("MultProfiles " + _profiles, LogSeverity.VERBOSE); 

			yield break;
		}

		/* -- useful for debugging which profile fails to deserialize
		IEnumerator Start()
		{
			string[] ids = "put,your,ids,here".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			foreach(string id in ids)
			{
				ProfileBehavior.ProfileIdentifiers = new string[] {id };
				yield return StartCoroutine(ProfileBehavior.ExecuteAndWait());
				
				_profiles = ProfileBehavior.Profiles;
				Debug.Log ("MultProfiles-recursive call is done with status : " + ProfileBehavior.Status);
				Debug.Log ("MultProfiles-recursive" + _profiles); 
			}

			yield break;
		}
		*/
	}
}
