using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.operations;
using hg.ApiWebKit.apis.google.mappers;

namespace hg.ApiWebKit.apis.google
{
	public class GoogleInterceptedOperation
	{
		public GoogleOperation Operation;
		public Action<GoogleOperation, HttpResponse> OnSuccess = null; 
		public Action<GoogleOperation, HttpResponse> OnFailure = null;
		public Action<GoogleOperation, HttpResponse> OnComplete = null;
		public string[] Parameters;
	}
}
