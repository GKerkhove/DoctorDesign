#if UNITY_5_3
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using UnityEngine.Experimental.Networking;

namespace hg.ApiWebKit.providers
{
	public class HttpUnityWebRequestClient : HttpAbstractProvider
	{
		protected override IEnumerator sendImplementation ()
		{
			_uwr = new UnityWebRequest(
				Request.RequestModelResult.Uri, 
				Request.RequestModelResult.Verb,
				new DownloadHandlerBuffer(),
				new UploadHandlerRaw((Request.RequestModelResult.Data == null || Request.RequestModelResult.Data.Length == 0) 
		 			? new byte[] { 0x0 } :  Request.RequestModelResult.Data));

			foreach(KeyValuePair<string,string> de in Request.RequestModelResult.Headers)
			{
				_uwr.SetRequestHeader(de.Key,de.Value);
			}

			_uwr.Send();

			while(TimeElapsed <= Request.RequestModelResult.Timeout && !_uwr.isDone && !RequestCancelFlag)
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
			return (Request.RequestModelResult.Verb == "GET") ? _uwr.downloadProgress : _uwr.uploadProgress;
		}

		protected override void disposeInternal()
		{
			if (_uwr != null)
			{
				_uwr.Abort();   // TODO: might need to move out of here
				_uwr.Dispose();
				_uwr = null;
			}
		}

		protected override Dictionary<string,string> getResponseHeaders ()
		{
			return (_uwr == null) ? null : _uwr.GetResponseHeaders();
		}


		protected override string getError ()
		{
			return (_uwr == null) ? null : _uwr.error;
		}


		protected override string getText ()
		{
			return (_uwr == null) 
				? null 
				: (string.IsNullOrEmpty(_uwr.error)) 
					? _uwr.downloadHandler==null 
						? null 
						: _uwr.downloadHandler.text 
					: null; 
		}


		protected override byte[] getData ()
		{
			return (_uwr == null) 
				? null 
				: (string.IsNullOrEmpty(_uwr.error)) 
					? _uwr.downloadHandler==null 
						? null 
						: _uwr.downloadHandler.data 
					: null; 
		}

		protected override HttpStatusCode getStatusCode()
		{
			return (HttpStatusCode) _uwr.responseCode;
		}

		private UnityWebRequest _uwr = null;
	}
}
#endif