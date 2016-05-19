using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class JobFacet : ModelBase
	{
		public string code;
		
		public string name;
		
		public Buckets buckets;
	}
}

