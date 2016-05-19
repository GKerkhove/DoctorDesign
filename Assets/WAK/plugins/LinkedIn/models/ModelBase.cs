using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public abstract class ModelBase
	{
		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}