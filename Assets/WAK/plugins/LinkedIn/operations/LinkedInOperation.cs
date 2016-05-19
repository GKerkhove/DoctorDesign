using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.linkedin.operations
{
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpUniWebClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpBestHttpClient))]
	public abstract class LinkedInOperation : HttpOperation
	{
		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}

