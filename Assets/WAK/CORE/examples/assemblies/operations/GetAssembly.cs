using UnityEngine;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;
using System.Reflection;


namespace hg.ApiWebKit.apis.example.assemblies.operations
{
	[HttpGET] 
	[HttpPath(null,"{$AssemblyUri}")]
	[HttpTimeout(30f)]
	[HttpContentType("text/html")]
	[HttpAccept("application/*")]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpFaultNon200]
	public class GetAssembly : ExampleOperation
	{
		[HttpUriSegment]
		public string AssemblyUri;

		[HttpResponseAssemblyBody]
		public Assembly ExternalAssembly;

		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}

