using UnityEngine;
using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.mappers;


namespace hg.ApiWebKit.apis.example.apiintro.operations
{

	[HttpGET]
	[HttpPath("hg.example","/WAK/v2/cars/{$make}")]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpAccept("application/json")]
	public class GetCarModelsV2Template: HttpOperation
	{
		[HttpUriSegment("make")]
		public string Make;

		[HttpResponseJsonBody]
		public models.CarModels Response;

		protected override void FromResponse(HttpResponse response)
		{
			base.FromResponse(response);
		}
	}
}

