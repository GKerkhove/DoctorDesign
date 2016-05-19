using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Message : ModelBase
	{
		public Message(string[] to, bool copySelf, string messageSubject, string messageBody)
		{
			recipients = new MessageRecipients();
			recipients.values = new MessageRecipientList();

			if(to != null)
			{
				foreach(string t in to)
				{
					if(!string.IsNullOrEmpty(t)) recipients.values.Add(new MessageRecipient(t));
				}
			}

			if(copySelf)
				recipients.values.Add(new MessageRecipient());

			subject = messageSubject;
			body = messageBody;
		}

		public MessageRecipients recipients;
		public string subject;
		public string body;
	}
}

