using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.models;
using hg.ApiWebKit.apis.haptix.editor;


namespace hg.ApiWebKit.apis.google.editor
{
	// the device token consent editor mitigates device access token expiration, missing client info and consent
	[HaptixEditorWindowTitle("Google Consent")]
	public class OAuthDeviceAccessTokenConsentEditor : HaptixEditorWindow
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			
			// if this window was opened when Unity started up then close it
			if(!_thisWindowIsValid)
				requestEditorClose(false);
		}
		
		protected override void OnDisable()
		{
			base.OnDisable();
			
			// retry consent when this window closes, eg. user clicks the X and closes the window
			if(_requestRetryOnClose)
				Configuration.GetSetting<IGoogleConsentInterceptor>("google.oauth-consent-interceptor").RetryConsent();
		}
		
		
		private OAuthDeviceUserCode _userCode = null;
		private bool _consentCompletionSuccess = false;
		private string _consentCompletionReason = "";
		
		// prevent window from popping up when Unity starts up
		private bool _thisWindowIsValid = false;
		
		// IGoogleConsentInterceptor got a request from google interceptors for consent
		public void OnConsentRequested(OAuthDeviceUserCode userCode)
		{
			_thisWindowIsValid = true;
			
			_userCode = userCode;
				
			FSM.Goto(__tiny__ConsentPrompt());
		}
		
		// IGoogleConsentInterceptor got a notification from google interceptors on the consent completion status
		public void OnConsentCompleted(bool success, string reason)
		{
			_thisWindowIsValid = true;
		
			_consentCompletionSuccess = success;
			_consentCompletionReason = reason;
				
			FSM.Goto((_consentCompletionSuccess==true) ? __tiny__Success() : __tiny__Failure());
		}
		
		// open this editor from IGoogleConsentInterceptor
		public static OAuthDeviceAccessTokenConsentEditor WhenConsentRequested(OAuthDeviceUserCode userCode)
		{
			OAuthDeviceAccessTokenConsentEditor window = EditorWindow.GetWindow<OAuthDeviceAccessTokenConsentEditor>();
			
			window.Show();
			window.OnConsentRequested(userCode);
			
			return window;
		}
		
		// open this editor from IGoogleConsentInterceptor
		public static OAuthDeviceAccessTokenConsentEditor WhenConsentCompleted(bool success, string reason)
		{
			OAuthDeviceAccessTokenConsentEditor window = EditorWindow.GetWindow<OAuthDeviceAccessTokenConsentEditor>();
			
			window.Show();
			window.OnConsentCompleted(success,reason);
			
			return window;
		}
		
		
		bool _requestRetryOnClose = true;
		
		// if a user closes this window, it will request consent once more to open up again
		private void requestRetryOnClose(bool yesOrNo)
		{
			_requestRetryOnClose = yesOrNo;
		}
		
		// close this window
		private void requestEditorClose(bool requestConsentOnClose = true)
		{
			if(!requestConsentOnClose)
				requestRetryOnClose(false);
				
			try
			{
				this.Close();
			}
			catch {}
		}
		
		
		// displays the google consent prompt
		IEnumerator __tiny__ConsentPrompt()
		{
			requestRetryOnClose(true);
		
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					if(_userCode.IsExpired)
					{
						EditorGUILayout.HelpBox("User code has expired!",MessageType.Warning);
						
						if(FSM.CanTransition)
						{
							if(GUILayout.Button("Retry"))
							{
								requestEditorClose();
							}
						}	
					}
					else
					{
						EditorGUILayout.HelpBox("Click on Allow and paste the user code, or Cancel this operation.",MessageType.Info);
						
						EditorGUILayout.BeginVertical();
						
						EditorGUILayout.TextField("User Code:",_userCode.user_code);
						EditorGUILayout.TextField("Verification Url:",_userCode.verification_url);
						
						EditorGUILayout.BeginHorizontal();
						
						if(FSM.CanTransition)
						{
							if(GUILayout.Button("Cancel"))
							{
								Configuration.GetSetting<IGoogleConsentInterceptor>("google.oauth-consent-interceptor").CancelConsent();
								FSM.Goto(__tiny__Cancelled());
							}
						}
						
						if(GUILayout.Button("Allow"))
						{
							EditorGUIUtility.systemCopyBuffer = _userCode.user_code;
							Application.OpenURL(_userCode.verification_url);
						}
						
						EditorGUILayout.EndHorizontal();
						
						if(!_userCode.IsExpired)
							EditorGUILayout.LabelField("User code will expire in: " + _userCode.ExpiresInSeconds.ToString() + " seconds");
						
						EditorGUILayout.EndVertical();
						
					}
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
			}
			
			UPDATE_STATE:
			while(!FSM.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM.CurrentStateCompleted();
			yield return null;
		}
		
		
		// user cancelled the google consent
		IEnumerator __tiny__Cancelled()
		{
			requestRetryOnClose(true);
		
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					EditorGUILayout.HelpBox("You disallowed Google access.  Your requests to Google will continue to fail!",MessageType.Warning);
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Retry"))
						{
							requestEditorClose();
						}
					}
				});
			}
			
			ENTER_STATE:
			{
			}
			
			UPDATE_STATE:
			while(!FSM.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM.CurrentStateCompleted();
			yield return null;
		}
		
		
		// access token retrieved
		IEnumerator __tiny__Success()
		{
			requestRetryOnClose(false);
		
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					EditorGUILayout.HelpBox("Access Token Retrieved!",MessageType.Info);
					
					if(GUILayout.Button("OK"))
					{
						requestEditorClose();
					}
				});
			}
			
			ENTER_STATE:
			{
			}
			
			UPDATE_STATE:
			while(!FSM.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM.CurrentStateCompleted();
			yield return null;
		}
		
		// user code failed to retrieve or access token failed to retrieve
		IEnumerator __tiny__Failure()
		{
			requestRetryOnClose(true);
		
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					EditorGUILayout.HelpBox(_consentCompletionReason,MessageType.Info);
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Cancel"))
						{
							Configuration.GetSetting<IGoogleConsentInterceptor>("google.oauth-consent-interceptor").CancelConsent();
							FSM.Goto(__tiny__Cancelled());
						}
						
						if(GUILayout.Button("Retry"))
						{
							requestEditorClose();
						}
					}
				});
			}
			
			ENTER_STATE:
			{
			}
			
			UPDATE_STATE:
			while(!FSM.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM.CurrentStateCompleted();
			yield return null;
		}
	}
}