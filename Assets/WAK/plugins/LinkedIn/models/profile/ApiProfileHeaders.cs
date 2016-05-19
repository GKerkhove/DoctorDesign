using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class ApiProfileHeaders : ModelBase
	{
		public int _total;
		public ApiProfileHeader[] values;
	}
}