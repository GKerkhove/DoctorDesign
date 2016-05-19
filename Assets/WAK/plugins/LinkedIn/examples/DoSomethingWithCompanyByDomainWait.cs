using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{

	public class DoSomethingWithCompanyByDomainWait : MonoBehaviour
	{
		public behaviors.LinkedInCompanyByDomainBehavior CompanyBehavior;

		[SerializeField]
		private models.Companies _companies;

		[SerializeField]
		private string _companyDomain;

		IEnumerator Start()
		{
			Configuration.Log("CompanyByDomain Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			CompanyBehavior.CompanyDomain = _companyDomain;
			yield return StartCoroutine(CompanyBehavior.ExecuteAndWait());
			_companies = CompanyBehavior.Companies;

			Configuration.Log("CompanyByDomain call is done with status : " + CompanyBehavior.Status,LogSeverity.VERBOSE);
			Configuration.Log("CompanyByDomain" + _companies,LogSeverity.VERBOSE);

			yield break;
		}
	}
}
