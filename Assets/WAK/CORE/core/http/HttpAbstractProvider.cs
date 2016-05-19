using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hg.ApiWebKit.core.http
{
	[ExecuteInEditMode]
	public abstract class HttpAbstractProvider: MonoBehaviour
	{
		[SerializeField]
		private string _id;

		[SerializeField]
		private string _targetUri;

		[SerializeField]
		private float _transferProgress;

		[SerializeField]
		private float _elapsedTime;

		[SerializeField]
		private float _timeToLive;

		[SerializeField]
		private HttpRequestState _state;

		protected abstract IEnumerator sendImplementation();

		protected abstract float getTransferProgress();

		protected abstract void disposeInternal();

		protected abstract Dictionary<string,string> getResponseHeaders();

		protected abstract string getError();

		protected abstract string getText();

		protected abstract byte[] getData();

		protected abstract HttpStatusCode getStatusCode();

		public HttpRequestState CurrentState
		{
			get;
			private set;
		}
		
		public HttpRequestState PreviousState
		{
			get;
			private set;
		}
		
		public float TimeElapsed
		{
			get;
			private set;
		}
		
		public float TTL
		{
			get { return Request.RequestModelResult.Timeout - TimeElapsed; }	
		}
		
		public float TransferProgress
		{
			get;
			private set;
		}

		public bool IsIdle { get { return CurrentState == HttpRequestState.IDLE; } }
		public bool IsRunning { get { return (CurrentState == HttpRequestState.BUSY || CurrentState == HttpRequestState.STARTED); } }

		protected HttpRequest Request = null;

		private System.Action<HttpResponse> _onCompleteCallback = null;
		private System.Action<float, float, float> _onTransferProgressUpdateCallback = null;
		private System.Action<HttpRequestState, HttpRequestState> _onStateChangeCallback = null;

		protected bool RequestCancelFlag = false;

		public bool Send(HttpRequest httpRequest, 
		                 System.Action<HttpResponse> onCompleteCallback,
		                 System.Action<float, float, float> onTransferProgressUpdateCallback,
		                 System.Action<HttpRequestState, HttpRequestState> onStateChangeCallback)
		{
			_id = httpRequest.RequestModelResult.TransactionId;
			_targetUri = httpRequest.RequestModelResult.Uri;

			Request = httpRequest;
			Request.Client = this;

			_onCompleteCallback = onCompleteCallback;
			_onTransferProgressUpdateCallback = onTransferProgressUpdateCallback;
			_onStateChangeCallback = onStateChangeCallback;
			
			if(!IsIdle) 
			{
				BehaviorComplete();
				return false;
			}
			
			bool disconnected = false;
			
			#if STRIPPED__UNITY_24_HOUR_DEAL || STRIPPED__UNITY_PUBLISHER
			Regex r = new Regex("unity3d.com|cloudfront.net|haptixgames.com");
			if(!r.IsMatch(Request.RequestModelResult.Uri.ToLower()))
				disconnected = true;
			#endif
			
			if (Application.internetReachability == NetworkReachability.NotReachable || disconnected)
			{
				ChangeState(HttpRequestState.DISCONNECTED);
				BehaviorComplete();
				return false;
			}
			
			ChangeState(HttpRequestState.STARTED);

			ChangeState(HttpRequestState.BUSY);

			#if UNITY_EDITOR
			_dirtyUp = Configuration.Bootstrap();
			UnityEditor.EditorApplication.update += this.Update;
			Configuration.Log("HttpAbstractProvider Editor Update Connected",LogSeverity.VERBOSE);
			_timingStartedAt = Time.realtimeSinceStartup;		
			#endif

			StartCoroutine(sendImplementation());
			
			return true;
		}

		protected void BehaviorComplete()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.update -= this.Update;
			Configuration.Log("HttpAbstractProvider Editor Update Disconnected",LogSeverity.VERBOSE);
			#endif
		
			Request.CompletionState = CurrentState;
			
			if(_onCompleteCallback != null)
			{
				_onCompleteCallback(new HttpResponse(
					Request, 
					TimeElapsed,
					getResponseHeaders(),
					getError(),
					getText(),
					getData(),
					getStatusCode()
				));
			}

			Dispose();
		}

		protected void ChangeState(HttpRequestState newState)
		{
			if(_onStateChangeCallback != null)
				_onStateChangeCallback(CurrentState, newState);
			
			PreviousState = CurrentState;
			CurrentState = newState;

			_state = CurrentState;
		}
		
		protected void UpdateTransferProgress()
		{
			TransferProgress = getTransferProgress();	

			_transferProgress = TransferProgress;
			_elapsedTime = TimeElapsed;
			_timeToLive = TTL;

			//TODO: throttle progress updates
			//TODO: implement auto-extend timeout if transfer is still happening

			if(_onTransferProgressUpdateCallback != null)
				_onTransferProgressUpdateCallback(TransferProgress, TimeElapsed, TTL);
		}

		public void Reset()
		{
			if(IsRunning)
				RequestCancelFlag = true;
			else
			{
				Cleanup(); 
				ChangeState(HttpRequestState.IDLE);
			}
		}
		
		public void Dispose()
		{
			Reset();
			ChangeState(HttpRequestState.NONE);

			if(Configuration.GetSetting<bool>("destroy-operation-on-completion"))
			{
				#if UNITY_EDITOR
				UnityEngine.Object.DestroyImmediate(this);
				#else
				UnityEngine.Object.Destroy(this);
				#endif
			}
		}
		
		protected void Cleanup()
		{
			TimeElapsed = 0f;
			disposeInternal();
		}

		public void Awake()
		{
			ChangeState(HttpRequestState.IDLE);	
		}
		
		public void Update()
		{
			if (IsRunning)
				#if UNITY_EDITOR
				TimeElapsed = Time.realtimeSinceStartup - _timingStartedAt;
				
				try
				{
					UnityEditor.EditorUtility.SetDirty(_dirtyUp);
				}
				catch(System.Exception ex) 
				{
					Configuration.Log("HttpAbstractProvider-editor encountered an error: " + ex, LogSeverity.WARNING);
				}
				#else
				TimeElapsed += Time.deltaTime;
				#endif
		}
		
		#if UNITY_EDITOR
		GameObject _dirtyUp;
		float _timingStartedAt = 0f;
		#endif
		
	}
}

