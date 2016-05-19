using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class InvitationList : ModelBase
	{
		public InviteeList values;
	}

	public class InviteeList : List<Invitee>
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
	
	[Serializable]
	public class Invitee : ModelBase
	{
		public InvitationRecipient person;
	}

	[Serializable]
	public class InvitationRecipient : ModelBase
	{
		public string _path;
		public string firstName;
		public string lastName;
	}

	[Serializable]
	public class InvitationItemContent : ModelBase
	{
		public InvitationRequest invitationRequest;
	}
	
	[Serializable]
	public class InvitationRequest : ModelBase
	{
		public string connectType;
		public InvitationAuthorization authorization;
	}

	[Serializable]
	public class InvitationAuthorization : ModelBase
	{
		public string name;
		public string value;
	}
}

