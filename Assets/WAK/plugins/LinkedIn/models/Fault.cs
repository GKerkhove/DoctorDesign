using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Fault : ModelBase
	{
		public int errorCode;
		public string message;
		public string requestId;
		public int status;
		public long timestamp;
	}
}