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
	[HttpPath("linkedin","/job-search{$fields}")]
	public class GetJobs : LinkedInOperationSecureGet
	{
		public enum JobSearchSortOrder
		{
			R,
			DA,
			DD
		}

		public GetJobs SearchCompany(string companyName)
		{
			CompanyName = companyName;
			return this;
		}

		public GetJobs SearchKeyword(string keywords)
		{
			Keywords = keywords;
			return this;
		}

		public GetJobs SearchJobTitle(string jobTitle)
		{
			JobTitle = jobTitle;
			return this;
		}

		public GetJobs SearchLocation(string countryCode, string postalCode, string distance)
		{
			CountryCode = countryCode;
			PostalCode = postalCode;
			Distance = distance;
			return this;
		}

		public GetJobs SetSort(JobSearchSortOrder order)
		{
			Sort = order.ToString();
			return this;
		}

		/* request */

		[HttpUriSegment(VariableValue = "linkedin.jobs.fieldSelectors")]
		public string fields;

		[HttpQueryString("keywords", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string Keywords;

		[HttpQueryString("job-title", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string JobTitle;

		[HttpQueryString("company-name", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string CompanyName;

		[HttpQueryString("country-code", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string CountryCode;

		[HttpQueryString("postal-code", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string PostalCode;

		[HttpQueryString("distance", Converter = typeof(hg.ApiWebKit.converters.Escape))]
		public string Distance;

		[HttpQueryString("start")]
		public string Start;
		
		[HttpQueryString("count")]
		public string Count;

		[HttpQueryString("sort")]
		public string Sort;

		/* response */

		[HttpResponseJsonBody]
		public models.Jobs jobs;
	}
}

