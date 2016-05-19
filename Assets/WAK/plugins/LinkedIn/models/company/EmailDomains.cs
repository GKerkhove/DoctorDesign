using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class EmailDomains : ModelBase
	{
		public int _total;

		public string[] values;

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(Utilities.InstanceToString(this));
			sb.AppendLine((values==null ? "(null)" : string.Join(",",values)));
			return sb.ToString();
		}
	}
}