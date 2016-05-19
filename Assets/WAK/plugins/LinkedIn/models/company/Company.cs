using UnityEngine;
using System;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Company : ModelBase
	{
		public float id;
		public string name;
		public string universalName;
		public string ticker;
		public string websiteUrl;
		public string logoUrl;
		public string squareLogoUrl;
		public string blogRssUrl;
		public string twitterId;
		public string industry;

		public string type;
		public string size;

		public int numFollowers;

		public Specialties specialties;
		public CompanyType companyType;
		public EmployeeCountRange employeeCountRange;
		public CompanyStatus status;
		public EmailDomains emailDomains;
	}
}

