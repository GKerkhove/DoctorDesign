using UnityEngine;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;
using System.Reflection;


namespace hg.ApiWebKit.apis.example.dummy.operations
{
	[HttpTimeout(12f)]
	[HttpGET] 
	[HttpPath(null,"{$SomeUri}")]
	[HttpContentType("text/html")]
	[HttpAccept("*/*")]
	[HttpFaultNon200]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpUniWebClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpBestHttpClient))]
	public class GETOperation : ExampleOperation
	{
		[HttpUriSegment]
		public string SomeUri;

		// we do nothing with the response

		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}

