using UnityEngine;
using System;
using System.Text;


namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class JobFacets : ModelBase
	{
		public int _total;
		
		public JobFacet[] values;
		
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

