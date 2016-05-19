using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithMyProfileNoWait : MonoBehaviour
	{
		public behaviors.LinkedInMyProfileBehavior ProfileBehavior;

		[SerializeField]
		private models.PersonList _profiles;

		void Start()
		{
			StartCoroutine(delayCall());
		}

		IEnumerator delayCall()
		{
			Configuration.Log("MyProfile Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			/*
			 * This script demonstrates invoking an operation without the use of coroutines.
			 */

			ProfileBehavior.OnCompleteNotification = 
				new Action<behaviors.LinkedInMyProfileBehavior, ApiBehaviorStatus> ((obj, status) => { 
					_profiles = obj.Profiles;
					Configuration.Log("Retrieved my profile with status : " + status, LogSeverity.VERBOSE);
					StartCoroutine(delayLog()); 
				});
			
			ProfileBehavior.Execute();

			// notice that this logging call will be executed immediately after the above call to Execute()
			//	we use the OnCompleteNotification callback to retrieve our response data
			Configuration.Log ("MyProfile requested...", LogSeverity.VERBOSE);
			
			yield break;
		}

		IEnumerator delayLog()
		{
			Configuration.Log("MyProfile Waiting to write to debug...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));
			
			Configuration.Log("MyProfile " + _profiles, LogSeverity.VERBOSE); 
			
			yield break;
		}

	}
}
