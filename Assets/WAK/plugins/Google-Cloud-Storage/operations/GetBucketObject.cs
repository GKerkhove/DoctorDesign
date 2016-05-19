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
using hg.ApiWebKit.apis.google.mappers;
using hg.ApiWebKit.apis.google.operations;

namespace hg.ApiWebKit.apis.google.cloudStorage.operations
{
	[HttpBearerAuthorization]
	[HttpPath("google.cloud-storage","/b/{$bucket}/o/{$object}")]
	[HttpGET]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class GetBucketObject : GoogleOperation
	{
		[HttpUriSegment("$bucket")]
		public string bucketName;
		
		[HttpUriSegment("$object")]
		public string objectName;
		
		[HttpResponseJsonBody]
		public models.ObjectResource response;
	
		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}
