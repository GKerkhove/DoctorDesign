using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace hg.ApiWebKit.apis.linkedin.models
{
	public class MessageRecipientList : List<MessageRecipient>
	{
		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(Utilities.InstanceToString(this));
			foreach(var p in this)
			{
				sb.AppendLine(Utilities.InstanceToString(p));
			}
			return sb.ToString();
		}
	}
}

