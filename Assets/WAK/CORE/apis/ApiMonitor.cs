using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis
{
	public class ApiMonitor: Singleton<ApiMonitor>
	{
		public void Aggregate(HttpResponse response)
		{
			if(response == null)
			{
				FailedCalls += 1;
				return;
			}

			BytesSent += response.Request.RequestModelResult.Data.Length;
			BytesReceived += response.Data == null ? 0 : response.Data.Length;
			SucceededCalls += response.HasError ? 0 : 1;
			FailedCalls += response.HasError ? 1 : 0;
			FaultedCalls += response.Request.RequestModelResult.Operation.IsFaulted ? 1 : 0;
			NetworkTime += response.TimeToComplete;
		}

		public float BytesSent;
		public float BytesReceived;
		public float SucceededCalls;
		public float FailedCalls;
		public float FaultedCalls;
		public float NetworkTime;
	}
}
