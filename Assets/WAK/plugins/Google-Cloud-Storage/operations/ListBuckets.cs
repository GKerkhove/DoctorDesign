using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.faulters;
using hg.ApiWebKit.apis.google.mappers;
using hg.ApiWebKit.apis.google.operations;
using hg.ApiWebKit.mappers;

namespace hg.ApiWebKit.apis.google.cloudStorage.operations
{
	[HttpBearerAuthorization]
	[HttpPath("google.cloud-storage","/b")]
	[HttpGET]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class ListBuckets : GoogleOperation
	{
		[HttpQueryString("project",VariableValue = "google.projectId")]
		public string projectId;
		
		[HttpResponseJsonBody]
		public models.BucketResources Buckets;
	
		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}

