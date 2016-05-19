using UnityEngine;

using System;
using System.IO;
using System.Collections;

using hg.ApiWebKit;
using System.Reflection;

namespace hg.ApiWebKit.apis.example.dummy
{
	public class TimeoutTest : MonoBehaviour
	{
		public string AbsoluteUri = "http://download.thinkbroadband.com/1GB.zip";

		IEnumerator Start()
		{
			Debug.Log ("Waiting...");
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));
			
			Debug.Log("Download...");

			new operations.GETOperation {
				SomeUri = AbsoluteUri
			}
			.Send(OnSuccess,OnFailure,OnComplete);

			yield break;
		}

		private void OnSuccess(operations.GETOperation operation, core.http.HttpResponse response)
		{
			Debug.Log("Success");
		}

		private void OnFailure(operations.GETOperation operation, core.http.HttpResponse response)
		{
			Debug.Log("Failed");

			Debug.Log("Faulted because: " + string.Join(" ; ", operation.FaultReasons.ToArray()));
		}

		private void OnComplete(operations.GETOperation operation, core.http.HttpResponse response)
		{
			Debug.Log("Completed with status code : " + response.StatusCode);
		}
	}
}
