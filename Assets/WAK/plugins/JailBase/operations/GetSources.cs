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
	[HttpPath(null,"http://www.jailbase.com/api/1/sources/")]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpAccept("application/json")]
	public class JailBaseSources : HttpOperation
	{
		//This operation returns a json containing all the sources used by jailbase.com
		[HttpResponseJsonBody]
		public models.Sources Response;

	}
}