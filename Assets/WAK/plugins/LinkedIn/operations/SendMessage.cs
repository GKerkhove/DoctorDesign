using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.linkedin.operations
{
	[HttpPath("linkedin","/people/~/mailbox")]
	public class SendMessage : LinkedInOperationSecurePost
	{
		/* request */

		[HttpRequestJsonBody]
		public models.Message message;

		protected override HttpRequest ToRequest (params string[] parameters)
		{
			return base.ToRequest (parameters);
		}

		protected override void OnRequestComplete (HttpResponse response)
		{
			base.OnRequestComplete (response);

			//Debug.Log(this.ToString());
		}
	}
}

