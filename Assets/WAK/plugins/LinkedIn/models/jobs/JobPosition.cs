using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class JobPosition : ModelBase
	{
		public JobExperienceLevel experienceLevel;
		public JobIndustries industries;
		public JobFunctions jobFunctions;
		public JobType jobType;
		public Location location;
		public string title;
	}
}

