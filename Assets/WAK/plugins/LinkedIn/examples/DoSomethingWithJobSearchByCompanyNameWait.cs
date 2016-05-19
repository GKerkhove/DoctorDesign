using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{

	public class DoSomethingWithJobSearchByCompanyNameWait : MonoBehaviour
	{
		public behaviors.LinkedInJobSearchByCompanyNameBehavior JobSearchBehavior;

		[SerializeField]
		private models.Jobs _jobs;

		[SerializeField]
		private string _companyName;

		IEnumerator Start()
		{
			Configuration.Log("JobByCompanyName Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			JobSearchBehavior.CompanyName = _companyName;
			yield return StartCoroutine(JobSearchBehavior.ExecuteAndWait());
			_jobs = JobSearchBehavior.Jobs;

			Configuration.Log("JobByCompanyName call is done with status : " + JobSearchBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("JobByCompanyName " + _jobs, LogSeverity.VERBOSE);

			yield break;
		}
	}
}
