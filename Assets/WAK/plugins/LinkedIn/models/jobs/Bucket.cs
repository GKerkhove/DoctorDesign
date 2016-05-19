using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Bucket : ModelBase
	{
		public string code;
		public int count;
		public string name;
		public bool selected;
	}
}

