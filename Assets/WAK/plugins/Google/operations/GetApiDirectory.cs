using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.faulters;
using hg.ApiWebKit.mappers;

namespace hg.ApiWebKit.apis.google.operations
{
	[HttpPath(null,"https://www.googleapis.com/discovery/v1/apis")]
	[HttpGET]
	[HttpTimeout(15f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class GetApiDirectory : HttpOperation
	{
		[HttpResponseJsonBody]
		public models.ApiDiscoveryDirectoryList Directory;
	
		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}