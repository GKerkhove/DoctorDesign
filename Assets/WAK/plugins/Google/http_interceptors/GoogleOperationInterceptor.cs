using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.tinyfsm;
using hg.ApiWebKit.tinyworkflow;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.operations;

namespace hg.ApiWebKit.apis.google
{
	public abstract class GoogleOperationInterceptor : MonoBehaviour, IGoogleOperationInterceptor
	{
		// http-ops to process as they come in
		protected Queue<GoogleInterceptedOperation> _queuedOperations;
		// http-ops to reprocess once they start failing
		protected Queue<GoogleInterceptedOperation> _failedOperations;
		// http-ops that are currently executing
		protected Dictionary<string,GoogleInterceptedOperation> _executingOperations;
		// http-ops that have executed
		//TODO remove
		protected Dictionary<string,GoogleInterceptedOperation> _completedOperations;
		
		protected TinyStateMachine	_fsm;
		protected System.Action<TinyStateMachine,string,string> onFsmStateChange = ((fsm,fromState,toState) => { });
		
		// do something when an intercepted http-op finishes
		protected System.Action<GoogleInterceptedOperation,GoogleOperation,HttpResponse> _onOperationCompletion = ((intercepted,operation,resposne) => {});
		
		
		protected virtual void Awake()
	 	{
			_queuedOperations = new Queue<GoogleInterceptedOperation>();
			_failedOperations = new Queue<GoogleInterceptedOperation>();
			_executingOperations = new Dictionary<string,GoogleInterceptedOperation>();
			_completedOperations = new Dictionary<string,GoogleInterceptedOperation>();
			
			//*** this should not be necessary since the state is referenced, unless an operation outside of the interceptor affects the cached token datas
			// reinitialize workflow state object to refresh data from player prefs
			/*onFsmStateChange = ((fromState,toState) => {
				_wfStateObject = new workflows.OAuthDeviceAccessTokenWorkflowStateObject();
			});*/
			
			_fsm = new TinyStateMachine(onFsmStateChange);
	 	}
	 	
		protected virtual void Start()
		{
			_fsm.Goto(__tiny__ValidateToken());
		}
		
		protected virtual void Update()
		{
			_fsm.Update();
		}
		
		protected virtual void OnDisable()
		{
			//TODO: error during playmode transitions 
			//  "Cannot destroy Component while GameObject is being activated or deactivated."
			//#if UNITY_EDITOR
			//Configuration.Log("[GoogleOperationInterceptor] Removing from scene.",LogSeverity.WARNING);
			//DestroyImmediate(this);
			//#endif
		}
		
		public bool PendingOAuthOperation
		{
			get
			{
				return !_fsm.IsInState(__tiny__AssumeValidToken());
			}
		}
		
		public bool IsExecutingOperations
		{
			get
			{
				return _executingOperations.Count > 0;
			}
		}
		
		public void Enqueue<T>(
			GoogleOperation operation, 
			Action<T, HttpResponse> on_success = null, 
			Action<T, HttpResponse> on_failure = null, 
			Action<T, HttpResponse> on_complete = null,
			params string[] parameters) where T : GoogleOperation
		{
			Configuration.Log("[GoogleOperationInterceptor] Adding operation to queue.",LogSeverity.VERBOSE);
			
			//TODO: MH17
			_queuedOperations.Enqueue(new GoogleInterceptedOperation {
				Operation = operation,
				OnSuccess = new Action<GoogleOperation,HttpResponse>( (o,r) => { on_success((T)o,r); }),
				OnFailure = new Action<GoogleOperation,HttpResponse>( (o,r) => { on_failure((T)o,r); }),
				OnComplete = new Action<GoogleOperation,HttpResponse>( (o,r) => { on_complete((T)o,r); }),
				Parameters = parameters
			});
		}
		
		// executed from a states Update method to begin executing some intercepted operation
		protected void executeInterceptedOperation(GoogleInterceptedOperation interceptedOperation)
		{
			// when the http-op starts, lets add it to a list of executing http-ops
			interceptedOperation.Operation["on-start"] = 
				new Action<GoogleOperation, HttpRequest>
					((self, request) => { 
						_executingOperations.Add(request.RequestModelResult.TransactionId, interceptedOperation);
					});
			
			// when the http-op completes, lets execute a state dependent completion event
			interceptedOperation.Operation["on-complete"] = 
				new Action<GoogleOperation, HttpResponse>
					((self, response) => { 
						_onOperationCompletion(interceptedOperation,self,response);
					});
			
			// send this http-op to the Google over wire
			interceptedOperation.Operation.Send(interceptedOperation.Parameters);
		}
		
		protected abstract IEnumerator __tiny__MissingClientInformation();
		protected abstract IEnumerator __tiny__AssumeValidToken();
		protected abstract IEnumerator __tiny__ResolveInvalidToken();
		protected abstract IEnumerator __tiny__ValidateToken();
		protected abstract IEnumerator __tiny__ConsentCancelled();
	}
}
