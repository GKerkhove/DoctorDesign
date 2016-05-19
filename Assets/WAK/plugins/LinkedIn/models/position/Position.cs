using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Position : ModelBase
	{
		public decimal id;
		public bool isCurrent;
		public string title;
		public StartDate startDate;
		public Company company;
	}
}

