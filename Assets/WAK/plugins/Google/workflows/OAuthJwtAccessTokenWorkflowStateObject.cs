using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using hg.ApiWebKit.tinyworkflow;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.models;

namespace hg.ApiWebKit.apis.google.workflows
{ 
	public class OAuthJwtAccessTokenWorkflowStateObject: WorkflowStateObject
	{
		public override void OnWorkflowStart ()
		{
			switch(Workflow.CurrentWorkflowName)
			{
				case "clear-jwt-access-token":
					models.OAuthJwtAccessToken.Clear();
					break;
					
				case "obtain-jwt-access-token":
					models.OAuthJwtAccessTokenClientInformation.Save(ClientInformation);
					break;
			}
		}
	
		public override void OnWorkflowStop()
		{
			//try { (this["access-token-polling-operation"] as HttpOperation).CancelRequest(); }
			//catch { }
		}
	
		public OAuthJwtAccessTokenWorkflowStateObject(params System.Action<bool>[] stepResultCallbacks): base(stepResultCallbacks)
		{
			RefreshDatas();
			
			TokenInformation = null;
		}
		
		public void RefreshDatas(bool tokenOnly = false)
		{
			if(!tokenOnly)
			{
				ClientInformation = OAuthJwtAccessTokenClientInformation.Load();
				
				if(ClientInformation==null)
					ClientInformation = new OAuthJwtAccessTokenMissingClientInformation();
			}
			
			CachedToken = models.OAuthJwtAccessToken.Load();
		}

		public models.OAuthJwtAccessTokenClientInformation	ClientInformation { get; private set; }
		public models.OAuthJwtAccessToken					CachedToken { get; set; }
		public models.OAuthTokenInfo 						TokenInformation { get; set; }
	}
}

