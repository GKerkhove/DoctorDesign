using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit.tinyworkflow;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google;

namespace hg.ApiWebKit.apis.google.workflows
{ 
	public class OAuthJwtAccessTokenWorkflow : Workflow<OAuthJwtAccessTokenWorkflowStateObject>
	{
		public OAuthJwtAccessTokenWorkflow()
		{
			Workflows =
				new Dictionary<string, List<Action<OAuthJwtAccessTokenWorkflowStateObject>>> 
				{
					{"obtain-jwt-access-token", new List<Action<OAuthJwtAccessTokenWorkflowStateObject>> 
						{
							OAuthJwtAccessTokenWorkflow.ObtainAccessTokenWorkflowStep
						}
					},
					{"clear-jwt-access-token", new List<Action<OAuthJwtAccessTokenWorkflowStateObject>>() },
					{"validate-jwt-access-token", new List<Action<OAuthJwtAccessTokenWorkflowStateObject>> 
						{
							OAuthJwtAccessTokenWorkflow.ValidateAccessTokenWorkflowStep
						}
					}
				};
		}
		
		public static System.Action<workflows.OAuthJwtAccessTokenWorkflowStateObject> ObtainAccessTokenWorkflowStep = ((state) => {
			
			GoogleJsonWebToken jwt = new GoogleJsonWebToken();
			
			new operations.GetOAuthJwtAccessToken {
				assertion = jwt.GetAssertion(state.ClientInformation)
			}.Send(
				new System.Action<operations.GetOAuthJwtAccessToken,HttpRequest>((operation,response) => {
					
				}),
				new System.Action<operations.GetOAuthJwtAccessToken,HttpResponse>((operation,response) => {
					models.OAuthJwtAccessToken.Save(operation.Token);
					state.RefreshDatas();
					state.Workflow.NextStep();
				}),
				new System.Action<operations.GetOAuthJwtAccessToken,HttpResponse>((operation,response) => {
					state.Workflow.StepComplete(false);
				}),
				new System.Action<operations.GetOAuthJwtAccessToken,HttpResponse>((operation,response) => {
				
				})
			);
		});
		
		public static System.Action<workflows.OAuthJwtAccessTokenWorkflowStateObject> ValidateAccessTokenWorkflowStep = ((state) => {
			
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
	}
}

