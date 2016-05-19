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
	public class OAuthDeviceAccessTokenWorkflowStateObject: WorkflowStateObject
	{
		public override void OnWorkflowStart ()
		{
			switch(Workflow.CurrentWorkflowName)
			{
				case "clear-device-access-token":
					models.OAuthDeviceAccessToken.Clear();
					break;
					
				case "obtain-device-access-token":
					models.OAuthDeviceAccessTokenClientInformation.Save(ClientInformation);
					break;
			}
		}
	
		public override void OnWorkflowStop()
		{
			try { (this["access-token-polling-operation"] as HttpOperation).CancelRequest(); }
			catch { }
		}
	
		public OAuthDeviceAccessTokenWorkflowStateObject(params System.Action<bool>[] stepResultCallbacks): base(stepResultCallbacks)
		{
			RefreshDatas();
			
			UserCode = null;
			TokenInformation = null;
		}
		
		public void RefreshDatas(bool tokenOnly = false)
		{
			if(!tokenOnly)
			{
				ClientInformation = OAuthDeviceAccessTokenClientInformation.Load();
				
				if(ClientInformation==null)
					ClientInformation = new OAuthDeviceAccessTokenMissingClientInformation();
			}
			
			CachedToken = models.OAuthDeviceAccessToken.Load();
		}

		public models.OAuthDeviceAccessTokenClientInformation		ClientInformation { get; private set; }
		public models.OAuthDeviceAccessToken				CachedToken { get; set; }
		public models.OAuthDeviceUserCode 					UserCode { get; set; }
		public models.OAuthTokenInfo 						TokenInformation { get; set; }
	}
}

