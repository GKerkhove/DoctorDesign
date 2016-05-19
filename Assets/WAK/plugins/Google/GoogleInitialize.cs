using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.google
{
	/// <summary>
	/// Include this class and prioritize its execution order to configure Google for your applicatoin.
	/// </summary>
	public class GoogleInitialize : ApiWebKitInitialize 
	{
		public bool LogVerbose = false;
		public bool LogInformation = true;
		public bool LogWarning = true;
		public bool LogError = true;


		/// <summary>
		/// General Google OAuth configuration for your application.
		/// </summary>
		[SerializeField] public models.OAuthConfiguration 					OAuthConfiguration = null; 

		/// <summary>
		/// Information supplied to Google to obtain a token when using DEVICE OAuth flow.
		/// </summary>
		[SerializeField] models.OAuthDeviceAccessTokenClientInformation 	OAuthDeviceClientInfo = null;
		
		/// <summary>
		/// Information supplied to Google to obtain a token when using SERVICE OAuth flow.
		/// </summary>
		[SerializeField] models.OAuthJwtAccessTokenClientInformation 		OAuthServiceClientInfo = null;
		
		
		/// <summary>
		/// Interception of Google OAuth flow events when using the DEVICE OAuth flow.
		/// </summary>
		public MonoBehaviour OAuthConsentInterceptor = null;
		
		public string ProjectId = "perfect-tape-656";

		public string CloudStorageBaseUri = "https://www.googleapis.com/storage/v1";

		public override void Start()
		{
			Configuration.SetSetting("log-VERBOSE", LogVerbose);
			Configuration.SetSetting("log-INFO", LogInformation);
			Configuration.SetSetting("log-WARNING", LogWarning);
			Configuration.SetSetting("log-ERROR", LogError);

			Configuration.SetSetting("google.oauth-configuration",OAuthConfiguration);
			
			Configuration.SetBaseUri("google.cloud-storage",CloudStorageBaseUri);
			Configuration.SetSetting("google.projectId",ProjectId);
			
			#if !UNITY_EDITOR
			// sping up a consent interceptor for use with DEVICE OAuth flow
			if(OAuthConfiguration.AuthorizationType==GoogleAuthorizationType.DEVICE)
				Configuration.SetSetting("google.oauth-consent-interceptor",OAuthConsentInterceptor as IGoogleConsentInterceptor);
				
			// store Google info into player prefs
			models.OAuthConfiguration.Save(OAuthConfiguration);	
			models.OAuthDeviceAccessTokenClientInformation.Save(OAuthDeviceClientInfo);
			models.OAuthJwtAccessTokenClientInformation.Save(OAuthServiceClientInfo);
			#endif 
			
			base.Start();
		}
	}
}