using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.linkedin
{
	public sealed class LinkedInProxy : Singleton<LinkedInProxy>
	{
		protected LinkedInProxy() {}

		public void ValidateAuthCodeRequest(string apiKey, string redirectUrl, HttpCallbacks<operations.GetAuthorizationCode> callbacks)
		{
			new operations.GetAuthorizationCode()
			.SetParameters(apiKey,redirectUrl)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void GetAccessToken(string authorizationCode, string apiKey, string secretKey, string redirectUrl, HttpCallbacks<operations.GetAuthenticationToken> callbacks)
		{
			new operations.GetAuthenticationToken()
			.SetParameters(authorizationCode,apiKey,secretKey,redirectUrl)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void SendInvitationByEmail(models.InviteByEmail invitee, string subject, string body, HttpCallbacks<operations.SendInvitation> callbacks)
		{
			new operations.SendInvitation()
			{
				invitation = new models.Invitation(invitee, subject, body)
			}
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void SendInvitationById(models.InviteById invitee, string subject, string body, HttpCallbacks<operations.SendInvitation> callbacks)
		{
			new operations.SendInvitation()
			{
				invitation = new models.Invitation(invitee, subject, body)
			}
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void SendMessage(string[] toIds, bool copySelf, string subject, string body, HttpCallbacks<operations.SendMessage> callbacks)
		{
			new operations.SendMessage() {
				message = new models.Message(toIds, copySelf, subject, body)
			}
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void GetMultipleProfiles(HttpCallbacks<operations.GetProfile> callbacks, bool SecureUrls, params string[] resourceIdentifiers)
		{
			new operations.GetProfile() {
				secureUrls = SecureUrls
			}
			.@Resources(resourceIdentifiers)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void GetImage(string url, HttpCallbacks<operations.GetImage> callbacks)
		{
			new operations.GetImage() {
				absoluteUrl  = url
			}
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void GetJobByCompany(string companyName, HttpCallbacks<operations.GetJobs> callbacks)
		{
			new operations.GetJobs()
			.SearchCompany(companyName)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void GetCompanyById(string id, HttpCallbacks<operations.GetCompany> callbacks)
		{
			new operations.GetCompany()
			.ById(id)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}
		
		public void GetCompanyByDomain(string domain, HttpCallbacks<operations.GetCompany> callbacks)
		{
			new operations.GetCompany()
			.ByDomain(domain)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}

		public void GetMyConnections(HttpCallbacks<operations.GetConnections> callbacks, string Start = null, string Count = null, string Modified = null, string ModifiedSince = null)
		{
			new operations.GetConnections() {
				start = Start, 
				count = Count,
				modified = Modified,
				modifiedSince = ModifiedSince
			}
			.Mine()
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}
		
		public void GetConnectionsForId(string id, HttpCallbacks<operations.GetConnections> callbacks, string Start = null, string Count = null, string Modified = null, string ModifiedSince = null)
		{
			new operations.GetConnections() {
				start = Start, 
				count = Count,
				modified = Modified,
				modifiedSince = ModifiedSince
			}
			.ById(id)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}
		
		public void GetConnectionsForUrl(string url, HttpCallbacks<operations.GetConnections> callbacks, string Start = null, string Count = null, string Modified = null, string ModifiedSince = null)
		{
			new operations.GetConnections() {
				start = Start, 
				count = Count,
				modified = Modified,
				modifiedSince = ModifiedSince
			}
			.ByUrl(url)
			.Send(callbacks.done,callbacks.fail,callbacks.always);
		}
	}
}