using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class InviteByEmail : ModelBase
	{
		public string Email;
		public string FistName;
		public string LastName;
	}

	[Serializable]
	public class InviteById : ModelBase
	{
		public string Id;
		public string AuthName;
		public string AuthValue;
	}

	[Serializable]
	public class Invitation : ModelBase
	{
		public Invitation(InviteByEmail invitee, string invitationSubject, string invitationBody)
		{
			recipients = new InvitationList();
			recipients.values = new InviteeList();

			if(invitee != null)
			{
				recipients.values.Add (new Invitee {
					person = new InvitationRecipient {
						_path = "/people/email=" + invitee.Email,
						firstName = invitee.FistName,
						lastName = invitee.LastName
					}
				});

				itemContent = new InvitationItemContent {
					invitationRequest = new InvitationRequest {
						connectType = "friend",
						authorization = new InvitationAuthorization {
							name = "",
							value = ""
						}
					}
				};

				subject = invitationSubject;
				body = invitationBody;
			}
		}

		public Invitation(InviteById invitee, string invitationSubject, string invitationBody)
		{
			recipients = new InvitationList();
			recipients.values = new InviteeList();
			
			if(invitee != null)
			{
				recipients.values.Add (new Invitee {
					person = new InvitationRecipient {
						_path = "/people/" + invitee.Id
					}
				});

				itemContent = new InvitationItemContent {
					invitationRequest = new InvitationRequest {
						connectType = "friend",
						authorization = new InvitationAuthorization {
							name = invitee.AuthName,
							value = invitee.AuthValue
						}
					}
				};

				subject = invitationSubject;
				body = invitationBody;
			}
		}

		public InvitationList recipients;
		public string subject;
		public string body;
		public InvitationItemContent itemContent;
	}
}

