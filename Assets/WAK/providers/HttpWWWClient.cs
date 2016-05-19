using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.providers
{
#if UNITY_5_3
	public sealed class HttpWWWClient : HttpUnityWebRequestClient
	{

	}
#else
	[Obsolete("Use hg.ApiWebKit.providers.UnityWebRequestClient instead.")]
	public class HttpWWWClient : HttpAbstractProvider
	{
		protected override IEnumerator sendImplementation ()
		{
			//TODO: REPLACE deprecated constructors

			if(Request.RequestModelResult.Verb == "GET")
				_www = new WWW(Request.RequestModelResult.Uri, null, Request.RequestModelResult.Headers);
			else if(Request.RequestModelResult.Verb == "POST")
				_www = new WWW(Request.RequestModelResult.Uri, 
				               ( 
				 				(Request.RequestModelResult.Data == null || Request.RequestModelResult.Data.Length == 0) 
				 					? new byte[] { 0x0 } :  Request.RequestModelResult.Data 
							   ), //HACK: add 1 byte if there are none.
				               Request.RequestModelResult.Headers); //TODO: replace sig

			else
				throw new NotSupportedException(Request.RequestModelResult.Verb + " verb is not supported by the HttpWWWClient.");

			while(TimeElapsed <= Request.RequestModelResult.Timeout && !_www.isDone && !RequestCancelFlag)
			{
				UpdateTransferProgress();
				yield return null;
			}
			
			UpdateTransferProgress();
			
			if(RequestCancelFlag)
			{
				RequestCancelFlag = false;	
				disposeInternal();	
				ChangeState(HttpRequestState.CANCELLED);
			}
			else if(TimeElapsed > Request.RequestModelResult.Timeout)
			{
				disposeInternal();
				ChangeState(HttpRequestState.TIMEOUT);
			}
			else
			{	
				if( ! string.IsNullOrEmpty(getError()) )
				{
					ChangeState(HttpRequestState.ERROR);
				}
				else
				{
					ChangeState(HttpRequestState.COMPLETED);
				}	
			}
			
			BehaviorComplete();
			
			Cleanup();
			
			yield break;
		}


		protected override float getTransferProgress ()
		{
			if (Request.RequestModelResult.Verb == "GET")
				return _www.progress;
			else if (Request.RequestModelResult.Verb == "POST")
				return _www.uploadProgress;
			else
				throw new NotSupportedException(Request.RequestModelResult.Verb + " verb is not supported by the WWWHttpClient.");
		}


		protected override void disposeInternal()
		{
			if (_www != null)
			{
				_www.Dispose();
				_www = null;
			}
		}


		protected override Dictionary<string,string> getResponseHeaders ()
		{
			if(_www == null)
				return null;

			Dictionary<string,string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach(KeyValuePair<string,string> kv in _www.responseHeaders)
			{
				headers.Add(kv.Key, kv.Value);
			}
			
			return headers;
		}


		protected override string getError ()
		{
			return (_www == null) ? null : _www.error;
		}


		protected override string getText ()
		{
			return (_www == null) ? null : (string.IsNullOrEmpty(_www.error)) ? _www.text : null; 
		}


		protected override byte[] getData ()
		{
			return (_www == null) ? null : (string.IsNullOrEmpty(_www.error)) ? _www.bytes : null;
		}

		protected override HttpStatusCode getStatusCode()
		{
			Dictionary<string,string> headers = getResponseHeaders();
			HttpStatusCode statusCode = HttpStatusCode.Unknown;
			
			if(headers==null)
			{
				Request.RequestModelResult.Operation.Log("Unable to parse HTTP status code because response headers could not be found.", LogSeverity.WARNING);
				return statusCode;
			}

			if (headers.ContainsKey ("STATUS")) {
				string status = headers ["STATUS"];
				statusCode = HttpStatusCode.Unknown;

				if (!string.IsNullOrEmpty (status)) {
					int code = 0;
					string[] statusSplit = status.Split (' ');
					if (!int.TryParse (statusSplit [1], out code))
						Request.RequestModelResult.Operation.Log ("Unable to parse HTTP status code from STATUS header.", LogSeverity.WARNING);
					else
						statusCode = (HttpStatusCode)code;
				} else
					Request.RequestModelResult.Operation.Log ("Unable to parse HTTP status code because STATUS response header could not be found.", LogSeverity.WARNING);
			} else if (Request.CompletionState == HttpRequestState.COMPLETED && string.IsNullOrEmpty (getError ())) {
				Request.RequestModelResult.Operation.Log ("Unable to parse HTTP status code.", LogSeverity.INFO);

#if UNITY_IOS
				Request.RequestModelResult.Operation.Log("Assumed HTTP status code = 202.", LogSeverity.WARNING);	
				statusCode = HttpStatusCode.Accepted;  
				//HACK: this is a hack for ios since it does NOT return STATUS header. 
				//TODO: fix in trampoline
#endif
			} 
			else 
			{					
				Request.RequestModelResult.Operation.Log ("Unable to parse HTTP status code.", LogSeverity.WARNING);
			}

			return statusCode;
		}

		private WWW _www = null;
	}
#endif
}