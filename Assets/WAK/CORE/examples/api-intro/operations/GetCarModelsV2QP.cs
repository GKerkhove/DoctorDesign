using UnityEngine;
using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;


namespace hg.ApiWebKit.apis.example.apiintro.operations
{

	[HttpGET]
	[HttpPath("hg.example","/WAK/v2/cars")]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpAccept("application/json")]
	[HttpFaultWhen("Response.model", HttpFaultWhenCondition.Is, null)]
	public class GetCarModelsV2QP: HttpOperation
	{
		[HttpQueryString("make")]
		public string Make;

		[HttpQueryString("sort")]
		public string SortOrder;

		[HttpResponseJsonBody]
		public models.CarModels Response;

		protected override void FromResponse(HttpResponse response)
		{
			base.FromResponse(response);
		}
	}
}

