using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.converters;
using System;

namespace hg.ApiWebKit.apis.jailbase.operations
{
	[HttpGET]
	[HttpPath(null,"http://www.jailbase.com/api/1/{$method}/")]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpAccept("application/json")]
	public class RecentAndSearch : HttpOperation
	{
		public RecentAndSearch()
		{
			hg.LitJson.JsonMapper.RegisterImporter<Int32,String>(
				new hg.LitJson.ImporterFunc<Int32,String> ((intValue) => { return intValue.ToString(); } )
			);
		}
	
		public RecentAndSearch DoSearch(string source, string lastName, string firstName)
		{
			method = "search";
			source_id = source;
			last_name = lastName;
			first_name = firstName;
			
			return this;
		}
		
		public RecentAndSearch GetRecent(string source)
		{
			method = "recent";
			source_id = source;
			
			return this;
		}
	
		protected override void FromResponse (HttpResponse response)
		{
			base.FromResponse (response);
		}
	
		[HttpUriSegment]
		public string method;

		//source_id is the organization they collect info from such as: il-ccso - illinois cook county sheriffs office
		[HttpQueryString]
		public string source_id;

		[HttpQueryString]
		public string last_name;

		[HttpQueryString]
		public string first_name;

		[HttpQueryString]
		public string page;

		//pull the data as text, strip out the Details information and then convert it to json.
		//[HttpResponseTextBody(Converters = new Type[] { typeof(StripJailbaseDetails), typeof(DeserializeLitJson) })]
		//public Inmates Response;
		
		[HttpResponseJsonBody]
		public models.Inmates Response;
	}
}