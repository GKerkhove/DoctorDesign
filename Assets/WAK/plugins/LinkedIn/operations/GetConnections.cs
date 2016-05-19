using UnityEngine;

using System;
using System.Text;
using System.Collections;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.linkedin.operations
{
	[HttpPath("linkedin","/people/{$profileId}/connections{$fields}")]
	public class GetConnections : LinkedInOperationSecureGet
	{
		public GetConnections Mine()
		{
			profileId = "~";
			return this;
		}

		public GetConnections ById(string id)
		{
			profileId = "id="+id;
			return this;
		}

		public GetConnections ByUrl(string url)
		{
			profileId = "url="+System.Uri.EscapeUriString(url);
			return this;
		}

		/* request */

		[HttpUriSegment]
		public string profileId;

		[HttpUriSegment(VariableValue = "linkedin.connections.fieldSelectors")]
		public string fields;

		[HttpQueryString]
		public string start;

		[HttpQueryString]
		public string count;

		[HttpQueryString]
		public string modified;

		[HttpQueryString("modified-since")]
		public string modifiedSince;

		/* response */

		[HttpResponseJsonBody]
		public models.Connections connections;
	}
}

