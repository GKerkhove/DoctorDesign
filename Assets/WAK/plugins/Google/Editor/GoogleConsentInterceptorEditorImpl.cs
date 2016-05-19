using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.models;

namespace hg.ApiWebKit.apis.google.editor
{
	// intercept OAuth consent requests within editor
	public class GoogleConsentInterceptorEditorImpl: IGoogleConsentInterceptor
	{
		// called when google interceptor can not find client information
		public Action OnMissingClientInformation {
			get {
				return new Action (() => {
					// reopen the token editor
					OAuthAccessTokenEditor.ReOpen();
					// let the token consent editor know that we're missing info
					OAuthDeviceAccessTokenConsentEditor.WhenConsentCompleted(false, "Google Client information is missing.  Fill in the client information, click Save in the Token Editor, and then click Retry in the Constent Editor.");
				});
			}
		}
	
		//TODO: deprecate
		public Action<string, string> OnUserConsentRequest {
			get {
				return new Action<string,string> ((userCode,verificationUrl) => { });
			}
		}
		
		// google interceptors have identified an invalid access token and are letting us know that a user code is available for verification
		public Action<OAuthDeviceUserCode> OnUserConsentRequest2 {
			get {
				return new Action<OAuthDeviceUserCode> ((userCode) => {
					
					/*Configuration.Log("GOOGLE REQUIRES CONSENT!", LogSeverity.ERROR);
					Configuration.Log("Visit: " + userCode.verification_url, LogSeverity.ERROR);
					Configuration.Log("User Code: " + userCode.user_code, LogSeverity.ERROR);*/
					
					// open the token consent editor and pass in the user code	
					OAuthDeviceAccessTokenConsentEditor.WhenConsentRequested(userCode);
				});
			}
		}
		
		// google interceptors notify us once the entire consent workflow is complete
		public Action<bool> OnUserConsentComplete {
			get {
				return new Action<bool> ((success) => {
				
					//Configuration.Log("GOOGLE CONSENT STATUS: " + success, LogSeverity.INFO);
					
					// close the token consent editor if open
					OAuthDeviceAccessTokenConsentEditor.ForceClose();
					// reopen the token consent editor with a completion status code
					OAuthDeviceAccessTokenConsentEditor.WhenConsentCompleted(success, (success=true) ? "OK!" : "Check your Google Client information and try again.");
				});
			}
		}
		
		// calls into google interceptors
		public Action CancelConsent { get; set; }
		// calls into google interceptors
		public Action RetryConsent { get; set; }
	}
}