using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	public class MessageRecipient : ModelBase
	{
		public MessageRecipient()
		{
			person = new MessageRecipientPath { _path =  "/people/~" };
		}
		
		public MessageRecipient(string id)
		{
			person = new MessageRecipientPath { _path = "/people/" + id };
		}
		
		public MessageRecipientPath person;
	}
}

