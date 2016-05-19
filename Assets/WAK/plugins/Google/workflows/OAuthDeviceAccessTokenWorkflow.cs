using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit.tinyworkflow;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.google.workflows
{ 
	public class OAuthDeviceAccessTokenWorkflow : Workflow<OAuthDeviceAccessTokenWorkflowStateObject>
	{
		public OAuthDeviceAccessTokenWorkflow()
		{
			Workflows =
				new Dictionary<string, List<Action<OAuthDeviceAccessTokenWorkflowStateObject>>> 
				{
					{"obtain-device-access-token", new List<Action<OAuthDeviceAccessTokenWorkflowStateObject>> 
						{
							OAuthDeviceAccessTokenWorkflow.ObtainUserCodeWorkflowStep,
							OAuthDeviceAccessTokenWorkflow.ObtainAccessTokenWorkflowStep
						}
					},
					{"clear-device-access-token", new List<Action<OAuthDeviceAccessTokenWorkflowStateObject>>() },
					{"validate-device-access-token", new List<Action<OAuthDeviceAccessTokenWorkflowStateObject>> 
						{
							OAuthDeviceAccessTokenWorkflow.ValidateAccessTokenWorkflowStep
						}
					},
					{"refresh-device-access-token", new List<Action<OAuthDeviceAccessTokenWorkflowStateObject>> 
						{
							OAuthDeviceAccessTokenWorkflow.RefreshAccessTokenWorkflowStep
						}
					}
				};
		}
		
		public static System.Action<workflows.OAuthDeviceAccessTokenWorkflowStateObject> RefreshAccessTokenWorkflowStep = ((state) => {
			
			new operations.RefreshOAuthDeviceAccessToken {
				client_id = state.ClientInformation.ClientId,
				client_secret = state.ClientInformation.Secret,
				refresh_token = state.CachedToken.RefreshToken
			}.Send(
				new System.Action<operations.RefreshOAuthDeviceAccessToken,HttpResponse>((operation,response) => {
					models.OAuthDeviceAccessToken.Save(operation.Token);
					state.RefreshDatas();
					state.Workflow.NextStep();
				}),
				new System.Action<operations.RefreshOAuthDeviceAccessToken,HttpResponse>((operation,response) => {
					state.Workflow.StepComplete(false);
				}),
				new System.Action<operations.RefreshOAuthDeviceAccessToken,HttpResponse>((operation,response) => {
					
				})
			);
		});
		
		public static System.Action<workflows.OAuthDeviceAccessTokenWorkflowStateObject> ValidateAccessTokenWorkflowStep = ((state) => {
			
			new operations.GetOAuthTokenInfo {
				access_token = (state.CachedToken==null) ? "invalid_token" : state.CachedToken.AccessToken
			}.Send(
				new System.Action<operations.GetOAuthTokenInfo,HttpResponse>((operation,response) => {
					state.TokenInformation = operation.TokenInformation;
					state.Workflow.NextStep();
				}),
				new System.Action<operations.GetOAuthTokenInfo,HttpResponse>((operation,response) => {
					state.TokenInformation = null;
					state.Workflow.StepComplete(false);
				}),
				new System.Action<operations.GetOAuthTokenInfo,HttpResponse>((operation,response) => {
					
				})
			);
		});
		
		public static System.Action<workflows.OAuthDeviceAccessTokenWorkflowStateObject> ObtainUserCodeWorkflowStep = ((state) => 
		{	
			new operations.GetOAuthDeviceUserCode {
				client_id = state.ClientInformation.ClientId,
				scope = state.ClientInformation.Scope
			}.Send(
				new System.Action<operations.GetOAuthDeviceUserCode,HttpResponse>((operation,response) => {
					state.UserCode = operation.Code;
					state.Workflow.NextStep();
				}),
				new System.Action<operations.GetOAuthDeviceUserCode,HttpResponse>((operation,response) => {
					state.Workflow.StepComplete(false);
				}),
				new System.Action<operations.GetOAuthDeviceUserCode,HttpResponse>((operation,response) => {
					
				})
			);
		});
		
		public static System.Action<workflows.OAuthDeviceAccessTokenWorkflowStateObject> ObtainAccessTokenWorkflowStep = ((state) => {
			state.Workflow.StartCoroutine(pollAccessToken(state));
		});
		
		private static IEnumerator pollAccessToken(workflows.OAuthDeviceAccessTokenWorkflowStateObject state)
		{
			#if UNITY_EDITOR
			float timeout = Time.realtimeSinceStartup + state.UserCode.interval;
			
			state.Workflow.SubscribeEditorForUpdates();
			
			while(Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}
			
			state.Workflow.UnsubscribeEditorFromUpdates();
			
			#else
			yield return new WaitForSeconds(state.UserCode.interval);
			#endif
			
			new operations.GetOAuthDeviceAccessToken {
				client_id = state.ClientInformation.ClientId,
				client_secret = state.ClientInformation.Secret,
				code = state.UserCode.device_code
			}.Send(
				new System.Action<operations.GetOAuthDeviceAccessToken,HttpRequest>((operation,response) => {
					state["access-token-polling-operation"] = operation;
				}),
				new System.Action<operations.GetOAuthDeviceAccessToken,HttpResponse>((operation,response) => {
					models.OAuthDeviceAccessToken.Save(operation.Token);
					state.RefreshDatas();
					state.Workflow.NextStep();
				}),
				new System.Action<operations.GetOAuthDeviceAccessToken,HttpResponse>((operation,response) => {
					if(!response.Request.WasCancelled)
					{
						if(response.Is2XX)
							state.Workflow.RepeatStep();
						else
							state.Workflow.StepComplete(false);
					}
				}),
				new System.Action<operations.GetOAuthDeviceAccessToken,HttpResponse>((operation,response) => {
					state["access-token-polling-operation"] = null;
				})
			);
			
			yield return null;
		}
	}
}

