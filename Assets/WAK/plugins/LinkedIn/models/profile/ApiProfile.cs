using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class ApiProfile : ModelBase
	{
		public ApiProfileHeaders headers;
		public string url;
	}
}