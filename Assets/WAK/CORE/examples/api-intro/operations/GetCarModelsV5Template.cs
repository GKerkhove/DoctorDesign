using UnityEngine;
using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;


namespace hg.ApiWebKit.apis.example.apiintro.operations
{

	[HttpGET]
	[HttpPath("hg.example","/WAK/v5/cars/{$make}")]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpAccept("application/json")]
	[HttpFaultWhen("Fault.fault.message", HttpFaultWhenCondition.IsNot, null)]
	public class GetCarModelsV5Template: HttpOperation
	{
		[HttpUriSegment("make")]
		public string Make;

		[HttpResponseJsonBody]
		public models.CarModels Response;

		[HttpResponseJsonBody]
		public MessageFault Fault;

		protected override void FromResponse(HttpResponse response)
		{
			base.FromResponse(response);
		}
	}

	public class MessageFault
	{
		public Fault fault;
	}

	public class Fault
	{
		public string message;
	}
}

