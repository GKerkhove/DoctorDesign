using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{

	public class DoSomethingWithCompanyByIdWait : MonoBehaviour
	{
		public behaviors.LinkedInCompanyByIdBehavior CompanyBehavior;

		[SerializeField]
		private models.Company _company;

		[SerializeField]
		private string _companyId;

		IEnumerator Start()
		{
			Configuration.Log("CompanyById Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			CompanyBehavior.CompanyId = _companyId;
			yield return StartCoroutine(CompanyBehavior.ExecuteAndWait());
			_company = CompanyBehavior.Company;

			Configuration.Log("CompanyById call is done with status : " + CompanyBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("CompanyById " + _company, LogSeverity.VERBOSE);

			yield break;
		}
	}
}
