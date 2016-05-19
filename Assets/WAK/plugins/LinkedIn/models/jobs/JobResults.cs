using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class JobResults : ModelBase
	{
		public int _count;
		public int _start;
		public int _total;
		
		public Job[] values;
		
		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(Utilities.InstanceToString(this));
			if(values!=null)
			{
				foreach(var v in values)
				{
					sb.AppendLine(Utilities.InstanceToString(v));
				}
			}
			return sb.ToString();
		}
	}
}

