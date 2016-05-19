using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Job : ModelBase
	{
		public bool active;
		public string customerJobCode;
		public Company company;
		public string description;
		public string descriptionSnippet;
		public JobDate expirationDate;
		public Int64 expirationTimestamp;
		public float id;
		public JobPoster jobPoster;
		public string locationDescription;
		public JobPosition position;
		public JobDate postingDate;
		public Int64 postingTimestamp;
		public string siteJobUrl;
		public string skillsAndExperience;
	}
}

