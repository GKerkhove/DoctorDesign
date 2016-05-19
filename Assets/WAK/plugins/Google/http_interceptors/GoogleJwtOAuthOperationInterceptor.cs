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
	//TODO: after a project rebuild, remove this from scene or fix up the failed fsm
	
	/// <summary>
	/// Intercepts Google operations used with a SERVICE OAuth flow.
	/// </summary>
	[ExecuteInEditMode] public class GoogleJwtOAuthOperationInterceptor : GoogleOperationInterceptor
	{
		private IWorkflow 	_wf;
		private workflows.OAuthJwtAccessTokenWorkflowStateObject	_wfStateObject;

		protected override void Awake()
		{
			base.Awake();
				
			_wf = new workflows.OAuthJwtAccessTokenWorkflow();
			_wfStateObject = new workflows.OAuthJwtAccessTokenWorkflowStateObject();
		}



		// this state is bad.... the google client info cannot be found.... nothing we can do
		protected override IEnumerator __tiny__MissingClientInformation()
		{
			ENTER_STATE:
			{
				Configuration.Log("[Google Interceptor] (MISSING_CLIENT_INFO) Queued Operations:"  + _queuedOperations.Count.ToString() + ", Failed Operations:" + _failedOperations.Count.ToString() ,LogSeverity.INFO);
			}
			
			UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		// we're going to assume that we have a valid token and keep running ops
		protected override IEnumerator __tiny__AssumeValidToken()
		{
			ENTER_STATE:
			{
				// notification when an intercepted http-op completes
				_onOperationCompletion = ((intercepted,self,response) => {
					if(response.StatusCode==hg.ApiWebKit.HttpStatusCode.Unauthorized)
					{
						Configuration.Log("[GoogleOperationInterceptor] Operation [" + response.Request.RequestModelResult.TransactionId + "] failed and is being added to the failed queue." ,LogSeverity.INFO);
					
						// we sent the http-op with an invalid token
						//  so we're going to queue it in the failed http-ops
						//  and go to invalid token state

						_failedOperations.Enqueue(intercepted);

						if(!_fsm.NextStateRequested)
							_fsm.Goto(__tiny__ResolveInvalidToken());
					}
					else
					{
						Configuration.Log("[GoogleOperationInterceptor] Operation [" + response.Request.RequestModelResult.TransactionId + "] has completed." ,LogSeverity.VERBOSE);
						
					
						// the http-op came back OK!
						//  we're going to throw it on the completed stack
						_completedOperations.Add(response.Request.RequestModelResult.TransactionId,intercepted);

						// lets execute the user defined callbacks
						if(intercepted.OnSuccess != null && (response.Is2XX || response.Is100) && !self.IsFaulted)
							intercepted.OnSuccess(self, response);
						
						if(intercepted.OnFailure !=null && ((!response.Is2XX && !response.Is100) || self.IsFaulted))
							intercepted.OnFailure (self, response);
						
						if(intercepted.OnComplete !=null)
							intercepted.OnComplete(self, response);
					}

					// http-op has come to completion, lets dequeue it
					_executingOperations.Remove(response.Request.RequestModelResult.TransactionId);
				});
			}
			
			UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				// attampt to reprocess failed http-ops
				if(_failedOperations.Count > 0 && !_fsm.IsNextState(__tiny__ResolveInvalidToken()))
				{
					executeInterceptedOperation(_failedOperations.Dequeue());
				}

				// go through queued http-ops, once all failed ones are done reprocessing
				if(_failedOperations.Count == 0 && _queuedOperations.Count > 0 && !_fsm.IsNextState(__tiny__ResolveInvalidToken()))
				{
					executeInterceptedOperation(_queuedOperations.Dequeue());
				}

				// let all executing operations run until completion
				if(_fsm.IsNextState(__tiny__ResolveInvalidToken()))
				{
					while(_executingOperations.Count > 0)
					{
						yield return null;
					}
				}
				
				//TODO: allow g-interceptor to sleep after inactivity

				yield return null;
			}
			
			EXIT_STATE:
			{
				_onOperationCompletion = ((intercepted,self,response) => { Configuration.Log("[Google Interceptor] An operation completed at an unexpected time.",LogSeverity.ERROR); });
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		protected override  IEnumerator __tiny__ResolveInvalidToken()
		{
			ENTER_STATE:
			{
				if(_wfStateObject.ClientInformation is models.OAuthJwtAccessTokenMissingClientInformation)
				{
					Configuration.Log("[Google Interceptor] Client information is not available to begin the OAuth flow.",LogSeverity.ERROR);
					
					_fsm.Goto(__tiny__MissingClientInformation());
				}
				else
				{
					_wfStateObject.SetResultCallbacks(
						new System.Action<bool> ((success) =>   
					    { 
							_wf.Stop();
						
							//TODO: loops state on failure
							_fsm.Goto((success==true) ? __tiny__ValidateToken() : __tiny__ResolveInvalidToken());
						})
					);
					
					_wf.StartWorkflow("obtain-jwt-access-token",_wfStateObject);
				}
			}
			
			UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		// execute workflow that validates the existing token
		protected override  IEnumerator __tiny__ValidateToken()
		{
			ENTER_STATE:
			{
				_wfStateObject.SetResultCallbacks(
					new System.Action<bool> ((success) =>
				    {
						_wf.Stop();
						_fsm.Goto( (success==true) ? __tiny__AssumeValidToken() : __tiny__ResolveInvalidToken() );
					})
				);
				
				_wf.StartWorkflow("validate-jwt-access-token",_wfStateObject);
			}
			
			UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}



		// user consent action was cancelled, we sit idle
		protected override  IEnumerator __tiny__ConsentCancelled()
		{
			ENTER_STATE:
			{
				Configuration.Log("[Google Interceptor] (CONSENT_CANCELLED) Queued Operations:"  + _queuedOperations.Count.ToString() + ", Failed Operations:" + _failedOperations.Count.ToString() ,LogSeverity.INFO);
			}
			
			UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}
	}
}
