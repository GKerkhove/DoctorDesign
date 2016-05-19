using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Jobs : ModelBase
	{
		public JobFacets facets;
		public JobResults jobs;
		public int numResults;
	}
}

