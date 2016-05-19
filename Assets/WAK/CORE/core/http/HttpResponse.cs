using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace hg.ApiWebKit.core.http
{
	[Serializable]
	public class HttpResponse
	{
		public HttpRequest 					Request 		;

		public float 						TimeToComplete 	;
		public Dictionary<string,string> 	Headers 		;	// TODO : Serialize
		public bool 						HasError		;
		public string 						Error			;
		[Multiline(50)]
		public string 						Text			;
		[HideInInspector]
		public byte[] 						Data			;
		public HttpStatusCode				StatusCode		;

		public bool 						Is100			{ get { return (int)StatusCode == 100; } }
		public bool							Is2XX			{ get { return (int)StatusCode >= 200 && (int)StatusCode < 300; } }
		
		public HttpResponse(HttpRequest		request, 
							float 			timeToComplete,
							Dictionary<string,string>		responseHeaders, 
							string 			responseError,
							string 			responseText,
							byte[] 			responseData,
		                    HttpStatusCode	statusCode)
		{
			Request = request;
			TimeToComplete = timeToComplete;
			Headers = responseHeaders;
			Error = responseError;
			Text = responseText;
			Data = responseData;
			StatusCode = statusCode;

			if(Request.CompletionState == HttpRequestState.TIMEOUT)
				Request.RequestModelResult.Operation.Log("Request has timed out.", LogSeverity.ERROR);
			
			if(!string.IsNullOrEmpty(Error) || 
			   Request.CompletionState != HttpRequestState.COMPLETED || 
			   StatusCode == HttpStatusCode.Unknown)
					HasError = true;
			
			Action<HttpResponse> httpFinishCallback = Configuration.GetSetting<Action<HttpResponse>>("on-http-finish");
			if(httpFinishCallback != null) httpFinishCallback(this);
		}


		public string Summary()
		{
			string h = "";

			if(Headers != null)
				foreach(KeyValuePair<string,string> kv in Headers)
				{
					h += "\t<color=grey>Key: " + kv.Key + " Value: " + kv.Value + "</color>\n";
				}

			string f = "";

			if(Request.RequestModelResult.Operation.FaultReasons != null)
			{
				foreach(string fault in Request.RequestModelResult.Operation.FaultReasons)
				{
					f += fault + "\n";
				}
			}

			return
				"<color=white><b>HTTP Response</b></color>\n" +
					"<color=grey>Transaction-Id: </color><color=cyan>" + Request.RequestModelResult.TransactionId + "</color>\n" +
					"<color=grey>Verb: </color><color=cyan>" + Request.RequestModelResult.Verb + "</color>\n" +
					"<color=grey>Uri: </color><color=cyan>" + Request.RequestModelResult.Uri + "</color>\n" +
					"<color=grey>Request-Completion-State: </color><color=cyan>" + Request.CompletionState + "</color>\n" +
					"<color=grey>Status-Code: </color><color=cyan>(" + (int)StatusCode + ") " + StatusCode + "</color>\n" +
					"<color=grey>Is-100: </color><color=cyan>" + Is100 + "</color>\n" +
					"<color=grey>Is-200s: </color><color=cyan>" + Is2XX + "</color>\n" +
					"<color=grey>Time-To-Complete: </color><color=cyan>" + TimeToComplete + "</color>\n" +
					"<color=grey>Has-Error: </color><color=cyan>" + HasError + "</color>\n" +
					"<color=grey>Error-Text: </color><color=cyan>" + Error + "</color>\n" +
					"<color=grey>Is-Faulted: </color><color=cyan>" + Request.RequestModelResult.Operation.IsFaulted + "</color>\n" +
					"<color=grey>Fault-Reason: </color><color=cyan>" + f + "</color>\n" +
					"<color=grey>Data-Length: </color><color=cyan>" + ((Data == null) ? "(null)" : Data.Length.ToString()) + "</color>\n" +
					"<color=grey>Headers: </color><color=cyan>" + ((Headers == null) ? "(null)" : Headers.Count.ToString()) + "</color>\n" + 
					h;
		}
	}
}