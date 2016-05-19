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
	[HttpPath("linkedin","/people{$resource}{$fields}")]
	public class GetProfile : LinkedInOperationSecureGet
	{
		public GetProfile Resources(params string[] resourceIdentifiers)
		{
			if(resourceIdentifiers.Length>0)
				resource = resourceIdentifiers;

			return this;
		}

		/* request */

		[HttpUriSegment(Converter = typeof(converters.ResourceSelectorSyntax))]
		public object resource;

		[HttpUriSegment(VariableValue = "linkedin.profile.fieldSelectors")]
		public string fields;

		[HttpQueryString("secure-urls", Converter = typeof(hg.ApiWebKit.converters.ToLowerCase))]
		public bool secureUrls;

		/* response */

		[HttpResponseJsonBody]
		public models.PersonList profiles;
	}
}

