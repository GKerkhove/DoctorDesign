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
	public class SendInvitation : LinkedInOperationSecurePost
	{
		/* request */

		[HttpRequestJsonBody]
		public models.Invitation invitation;
	}
}

