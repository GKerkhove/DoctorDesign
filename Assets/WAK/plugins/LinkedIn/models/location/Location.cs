using UnityEngine;
using System;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Location : ModelBase
	{
		public string name;
		public Country country;
	}
}

