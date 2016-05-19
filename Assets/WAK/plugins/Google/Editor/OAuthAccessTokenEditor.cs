using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.haptix.editor;
using hg.ApiWebKit.tinyfsm;
using hg.ApiWebKit.apis.google.models;

namespace hg.ApiWebKit.apis.google.editor
{
	[HaptixEditorWindowTitle("Google OAuth")]
	public class OAuthAccessTokenEditor : HaptixEditorWindow
	{
		public static OAuthAccessTokenEditor ReOpen()
		{
			if(OAuthAccessTokenEditor.IsOpen)
			{
				EditorWindow.FocusWindowIfItsOpen<OAuthAccessTokenEditor>();
				return null;
			}
			else
				return HaptixEditorWindow.Show<OAuthAccessTokenEditor>();
		}
		
		
		protected override void OnEnableUnserialized ()
		{
			
		}
		
		protected override void OnEnableSerialized ()
		{
			
		}
		
		protected override void OnEnable()
		{
			FSM_Content = new TinyStateMachine(this,OnStateChange);
		
			base.OnEnable();
			
			WF_DeviceOAuth = new workflows.OAuthDeviceAccessTokenWorkflow();
			WF_JwtOAuth = new workflows.OAuthJwtAccessTokenWorkflow();	
			FSM.Goto(__tiny__SelectTokenMode());
		}
		
		protected override void OnDisable()
		{	
			FSM_Content.Stop(true);
			
			WF_DeviceOAuth.Stop();
			WF_JwtOAuth.Stop();
			
			base.OnDisable();
		}
		
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
		
		protected override void Update()
		{
			FSM_Content.Update();
		
			base.Update();
		}
		
		protected override void OnGUI()
		{
			base.OnGUI();
			
			FSM_Content.OnGUI();
		}
		
		private TinyStateMachine FSM_Content = null;
		private bool _contentFsmIsBusy = false;
		
		private hg.ApiWebKit.tinyworkflow.IWorkflow 					WF_DeviceOAuth;
		private workflows.OAuthDeviceAccessTokenWorkflowStateObject 	WF_StateObject_DeviceOAuth;
		
		private hg.ApiWebKit.tinyworkflow.IWorkflow 					WF_JwtOAuth;
		private workflows.OAuthJwtAccessTokenWorkflowStateObject 		WF_StateObject_JwtOAuth;
		
		
		
		IEnumerator __tiny__SelectTokenMode()
		{
			int previousSelectedOAuthMode = -1;
			int selectedOAuthMode = (int)OAuthConfiguration.Load().AuthorizationType;
			
			DEFINE_GUI:
			{
				FSM.SetGui(() => {
					if(_contentFsmIsBusy)
						GUI.enabled = false;
						
					EditorGUILayout.HelpBox("Select the Google OAuth model you wish to work with.\nSelecting an OAuth model also makes it the active Google OAuth model.",MessageType.None);
					
					selectedOAuthMode = GUILayout.SelectionGrid(selectedOAuthMode,new string[] {"NONE","SERVICE","DEVICE"},3);
					
					EditorGUILayout.HelpBox("Current Google OAuth flow model: " + ((GoogleAuthorizationType)selectedOAuthMode).ToString() ,MessageType.None);
					
					GUILayout.Space(15);
					                        
					if(FSM.CanTransition)
					{
						if(previousSelectedOAuthMode!=selectedOAuthMode)
						{
							switch(selectedOAuthMode)
							{
								case 0:
									FSM_Content.Goto(__tiny__Content__StartNoneTokenMode());
									break;
								
								case 1:
									FSM_Content.Goto(__tiny__Content__ServiceTokenMode_Start());
									break;
									
								case 2:
									FSM_Content.Goto(__tiny__Content__DeviceTokenMode_Start());
									break;
							}
							
							OAuthConfiguration.Save(new OAuthConfiguration { AuthorizationType = (GoogleAuthorizationType)selectedOAuthMode });
						
							previousSelectedOAuthMode=selectedOAuthMode;
						}
					}
					
					GUI.enabled = true;
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
		
		
		IEnumerator __tiny__Content__StartNoneTokenMode()
		{
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					EditorGUILayout.HelpBox("If you want your editor application to utilize Google OAuth, you must select a valid OAuth model.",MessageType.Warning);
					EditorGUILayout.HelpBox("SERVICE - This model utilizes a P12 certificate obtained from Google Apps to obtain a Google access token.  No user consent or intervention is required to intitally generate a token or to refresh an expired one.",MessageType.Info);
					EditorGUILayout.HelpBox("DEVICE - This model is akin to 'Netflix' authorization where you will be presented with a URL link and authorization code within the Unity editor.  To authorize your application within Unity you will need to follow the URL link and enter the provided code for Unity to obtain a Google access token.",MessageType.Info);
					EditorGUILayout.HelpBox("This editor window only affects your in-editor experience, including running your application in play-mode.  Once your application is built, you will need to review the Google Initializer class and set the appropriate OAuth model and client information there.",MessageType.Info);
				});
			}
			
			ENTER_STATE:
			{
				
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				
				yield return null;
			}
			
			EXIT_STATE:
			{
				
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		
		
		IEnumerator __tiny__Content__ServiceTokenMode_Start()
		{
			bool validating = false;
			bool foldout_clientInfo = false;
			bool foldout_tokenInfo = false;
			
			System.Action validateToken = new System.Action (()=> {
				if(validating)
					return;
				
				validating = true;
				
				WF_StateObject_JwtOAuth.SetResultCallbacks(
					new System.Action<bool> ((success) =>
				    {
						WF_JwtOAuth.Stop();
						validating = false;
					})
				);
				
				WF_JwtOAuth.StartWorkflow("validate-jwt-access-token",WF_StateObject_JwtOAuth);
			});
			
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					foldout_clientInfo = EditorGUILayout.Foldout(foldout_clientInfo,"Client Information");
					
					if(foldout_clientInfo)
					{	
						EditorGUILayout.BeginVertical();
						
						if(WF_StateObject_JwtOAuth.ClientInformation.HasEmptyFields)
							EditorGUILayout.HelpBox("Enter all of the information below.", MessageType.Warning);
						else
							EditorGUILayout.HelpBox("Below information is required to request a token.", MessageType.Info);
						
						Color og2 = GUI.color;
						Color alt = og2;
						
						if(WF_StateObject_JwtOAuth.ClientInformation is models.OAuthJwtAccessTokenMissingClientInformation)
							alt = Color.cyan;
						
						GUI.color = alt;
						
						WF_StateObject_JwtOAuth.ClientInformation.ClientIdEmail = EditorGUILayout.TextField("Client Id Email:",WF_StateObject_JwtOAuth.ClientInformation.ClientIdEmail);
						WF_StateObject_JwtOAuth.ClientInformation.Audience = EditorGUILayout.TextField("Audience:",WF_StateObject_JwtOAuth.ClientInformation.Audience);
						WF_StateObject_JwtOAuth.ClientInformation.CertificateResourceName = EditorGUILayout.TextField("P12 Resource Name:",WF_StateObject_JwtOAuth.ClientInformation.CertificateResourceName);
						WF_StateObject_JwtOAuth.ClientInformation.Scope = EditorGUILayout.TextField("Scope:",WF_StateObject_JwtOAuth.ClientInformation.Scope);
						
						GUI.color = og2;
						
						EditorGUILayout.HelpBox("P12 Resource Name is the filename of the P12 certificate.  Get the P12 from Google Apps, rename it with a '.bytes' extension and drop it into one of your Resource folders so that it can be accessed as a TextAsset.", MessageType.None);
						
						
						EditorGUILayout.EndVertical();
						
						EditorGUILayout.BeginHorizontal();
						
						if(!WF_StateObject_JwtOAuth.ClientInformation.HasEmptyFields)
						{
							if(GUILayout.Button("Save"))
							{
								models.OAuthJwtAccessTokenClientInformation.Save(WF_StateObject_JwtOAuth.ClientInformation);
								WF_StateObject_JwtOAuth.RefreshDatas();
								
								foldout_clientInfo = false;
								foldout_tokenInfo = true;
							}
						}
						
						if(GUILayout.Button("Clear"))
						{
							models.OAuthJwtAccessTokenClientInformation.Clear();
							WF_StateObject_JwtOAuth.RefreshDatas();
							
							foldout_tokenInfo = false;
						}
						
						EditorGUILayout.EndHorizontal();
					}
					
					EditorGUILayout.Space();
					
					EditorGUILayout.Space();
					
					foldout_tokenInfo = EditorGUILayout.Foldout(foldout_tokenInfo,"Access Token Information");
					
					if(foldout_tokenInfo)
					{
						if(WF_StateObject_JwtOAuth.CachedToken==null)
						{
							EditorGUILayout.HelpBox("Token does not exist in cache.",MessageType.Error);
							
							//check to see if the token has been generated from another place
							// refresh only token, since we're still inputting client info
							WF_StateObject_JwtOAuth.RefreshDatas(true);
							
							if(WF_StateObject_JwtOAuth.CachedToken!=null)
								validateToken();
						}
						else
						{
							EditorGUILayout.BeginVertical();
							EditorGUILayout.HelpBox("Token found!\nYou can generate a new one.",MessageType.Info);
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(WF_StateObject_JwtOAuth.CachedToken.TokenType + " Token: " + WF_StateObject_JwtOAuth.CachedToken.AccessToken);
							if(GUILayout.Button("Copy"))
								EditorGUIUtility.systemCopyBuffer = WF_StateObject_JwtOAuth.CachedToken.AccessToken;
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.LabelField("Obtained: " + WF_StateObject_JwtOAuth.CachedToken.Obtained);
							
							Color og = GUI.color;
							if(validating)
							{
								GUI.color = Color.cyan;
								EditorGUILayout.LabelField("Validating...");
							}
							else
							{
								if(WF_StateObject_JwtOAuth.TokenInformation==null)
								{
									GUI.color = Color.red;
									EditorGUILayout.LabelField("Token Validation Failed");
								}
								else
								{
									GUI.color = Color.green;
									EditorGUILayout.LabelField("Token Is Valid");
									GUI.color = Color.yellow;
									EditorGUILayout.LabelField("Expires in " + WF_StateObject_JwtOAuth.CachedToken.ExpiresInSeconds + " seconds");
								}
							}
							GUI.color = og;
							
							EditorGUILayout.EndVertical();
						}
						
						if(FSM.CanTransition)
						{
							EditorGUILayout.BeginHorizontal();
							
							if(!(WF_StateObject_JwtOAuth.ClientInformation is models.OAuthJwtAccessTokenMissingClientInformation))
							{
								if(GUILayout.Button("Obtain"))
								{
									_contentFsmIsBusy = true;
									
									WF_StateObject_JwtOAuth.SetResultCallbacks(	
										new System.Action<bool> ((success) =>   
									    { 
											WF_JwtOAuth.Stop();       
											FSM_Content.Goto((success==true) ? __tiny__Content__ServiceTokenMode_Start() : __tiny__Content__ServiceTokenMode_NoAccessToken());
										})
									);
									
									WF_JwtOAuth.StartWorkflow("obtain-jwt-access-token",WF_StateObject_JwtOAuth);
									FSM_Content.Goto(__tiny__Content__Waiting());
								}
							}
							
							if(WF_StateObject_JwtOAuth.CachedToken!=null)
							{
								if(GUILayout.Button("Validate"))
								{
									validateToken();
								}
							}
							
							if(GUILayout.Button("Clear"))
							{
								WF_StateObject_JwtOAuth.SetResultCallbacks();
								WF_JwtOAuth.StartWorkflow("clear-jwt-access-token",WF_StateObject_JwtOAuth);
								WF_JwtOAuth.Stop();
								FSM.Goto(__tiny__SelectTokenMode());
							}
							
							EditorGUILayout.EndHorizontal();
						}
					}
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
				// refresh our state object with new datas
				WF_StateObject_JwtOAuth = new workflows.OAuthJwtAccessTokenWorkflowStateObject();
				
				if(WF_StateObject_JwtOAuth.ClientInformation.HasEmptyFields)
					foldout_clientInfo = true;
				
				if(WF_StateObject_JwtOAuth.CachedToken==null)
				{
					if(!WF_StateObject_JwtOAuth.ClientInformation.HasEmptyFields)
						foldout_tokenInfo = true;
				}
				else
				{
					foldout_tokenInfo = true;
					
					WF_StateObject_JwtOAuth.SetResultCallbacks(
						new System.Action<bool> ((success) =>
					    {
							WF_JwtOAuth.Stop();
						})
					);
					
					WF_JwtOAuth.StartWorkflow("validate-jwt-access-token",WF_StateObject_JwtOAuth);
				}
				
				_contentFsmIsBusy = false;
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				
				yield return null;
			}
			
			EXIT_STATE:
			{
				
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		
		IEnumerator __tiny__Content__DeviceTokenMode_Start()
		{
			bool validating = false;
			bool foldout_clientInfo = false;
			bool foldout_tokenInfo = false;
			
			System.Action validateToken = new System.Action (()=> {
				if(validating)
					return;
				
				validating = true;
				
				WF_StateObject_DeviceOAuth.SetResultCallbacks(
					new System.Action<bool> ((success) =>
				    {
						WF_DeviceOAuth.Stop();
						validating = false;
					})
				);
				
				WF_DeviceOAuth.StartWorkflow("validate-device-access-token",WF_StateObject_DeviceOAuth);
			});
		
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					foldout_clientInfo = EditorGUILayout.Foldout(foldout_clientInfo,"Client Information");
					
					if(foldout_clientInfo)
					{	
						EditorGUILayout.BeginVertical();
						
						if(WF_StateObject_DeviceOAuth.ClientInformation.HasEmptyFields)
							EditorGUILayout.HelpBox("Enter all of the information below.", MessageType.Warning);
						else
							EditorGUILayout.HelpBox("Below information is required to request a token.", MessageType.Info);
						
						Color og2 = GUI.color;
						Color alt = og2;
						
						if(WF_StateObject_DeviceOAuth.ClientInformation is models.OAuthDeviceAccessTokenMissingClientInformation)
							alt = Color.cyan;
						
						GUI.color = alt;
						
						WF_StateObject_DeviceOAuth.ClientInformation.ClientId = EditorGUILayout.TextField("Client Id:",WF_StateObject_DeviceOAuth.ClientInformation.ClientId);
						WF_StateObject_DeviceOAuth.ClientInformation.Secret = EditorGUILayout.PasswordField("Secret:",WF_StateObject_DeviceOAuth.ClientInformation.Secret);
						WF_StateObject_DeviceOAuth.ClientInformation.Scope = EditorGUILayout.TextField("Scope:",WF_StateObject_DeviceOAuth.ClientInformation.Scope);
						
						GUI.color = og2;
						
						EditorGUILayout.EndVertical();
						
						EditorGUILayout.BeginHorizontal();
						
						if(!WF_StateObject_DeviceOAuth.ClientInformation.HasEmptyFields)
						{
							if(GUILayout.Button("Save"))
							{
								models.OAuthDeviceAccessTokenClientInformation.Save(WF_StateObject_DeviceOAuth.ClientInformation);
								WF_StateObject_DeviceOAuth.RefreshDatas();
								
								foldout_clientInfo = false;
								foldout_tokenInfo = true;
							}
						}
						
						if(GUILayout.Button("Clear"))
						{
							models.OAuthDeviceAccessTokenClientInformation.Clear();
							WF_StateObject_DeviceOAuth.RefreshDatas();
							
							foldout_tokenInfo = false;
						}
						
						EditorGUILayout.EndHorizontal();
					}
					
					EditorGUILayout.Space();
					
					EditorGUILayout.Space();
					
					foldout_tokenInfo = EditorGUILayout.Foldout(foldout_tokenInfo,"Access Token Information");
					
					if(foldout_tokenInfo)
					{
					
						if(WF_StateObject_DeviceOAuth.CachedToken==null)
						{
							EditorGUILayout.HelpBox("Token does not exist in cache.",MessageType.Error);
							
							//check to see if the token has been generated from another place
							// refresh only token, since we're still inputting client info
							WF_StateObject_DeviceOAuth.RefreshDatas(true);
							
							if(WF_StateObject_DeviceOAuth.CachedToken!=null)
								validateToken();
						}
						else
						{
							EditorGUILayout.BeginVertical();
							EditorGUILayout.HelpBox("Token found!\nYou can generate a new one or refresh the existing one.",MessageType.Info);
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(WF_StateObject_DeviceOAuth.CachedToken.TokenType + " Token: " + WF_StateObject_DeviceOAuth.CachedToken.AccessToken);
							if(GUILayout.Button("Copy"))
								EditorGUIUtility.systemCopyBuffer = WF_StateObject_DeviceOAuth.CachedToken.AccessToken;
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Refresh Token: " + WF_StateObject_DeviceOAuth.CachedToken.RefreshToken);
							if(GUILayout.Button("Copy"))
								EditorGUIUtility.systemCopyBuffer = WF_StateObject_DeviceOAuth.CachedToken.RefreshToken;
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.LabelField("Obtained: " + WF_StateObject_DeviceOAuth.CachedToken.Obtained);
							
							Color og = GUI.color;
							if(validating)
							{
								GUI.color = Color.cyan;
								EditorGUILayout.LabelField("Validating...");
							}
							else
							{
								if(WF_StateObject_DeviceOAuth.TokenInformation==null)
								{
									GUI.color = Color.red;
									EditorGUILayout.LabelField("Token Validation Failed");
								}
								else
								{
									GUI.color = Color.green;
									EditorGUILayout.LabelField("Token Is Valid");
									GUI.color = Color.yellow;
									EditorGUILayout.LabelField("Expires in " + WF_StateObject_DeviceOAuth.CachedToken.ExpiresInSeconds + " seconds");
								}
							}
							GUI.color = og;
							
							EditorGUILayout.EndVertical();
						}
						
						if(FSM.CanTransition)
						{
							EditorGUILayout.BeginHorizontal();
							
							if(!(WF_StateObject_DeviceOAuth.ClientInformation is models.OAuthDeviceAccessTokenMissingClientInformation))
							{
								if(GUILayout.Button("Obtain"))
								{
									_contentFsmIsBusy = true;
								
									WF_StateObject_DeviceOAuth.SetResultCallbacks(
										new System.Action<bool> ((success) => 
									    {
											FSM_Content.Goto((success==true) ? __tiny__Content__DeviceTokenMode_HaveUserCode() : __tiny__Content__DeviceTokenMode_NoUserCode());
										}),	
										new System.Action<bool> ((success) =>   
									    { 
											WF_DeviceOAuth.Stop();       
											FSM_Content.Goto((success==true) ? __tiny__Content__DeviceTokenMode_Start() : __tiny__Content__DeviceTokenMode_NoAccessToken());
										})
									);
									
									WF_DeviceOAuth.StartWorkflow("obtain-device-access-token",WF_StateObject_DeviceOAuth);
									FSM_Content.Goto(__tiny__Content__Waiting());
								}
							}
							
							if(WF_StateObject_DeviceOAuth.CachedToken!=null)
							{
								if(GUILayout.Button("Validate"))
								{
									validateToken();
								}
								
								if(GUILayout.Button("Refresh"))
								{
									_contentFsmIsBusy = true;
									
									WF_StateObject_DeviceOAuth.SetResultCallbacks(
										new System.Action<bool> ((success) => 
									    { 
											WF_DeviceOAuth.Stop();       
											FSM.Goto((success==true) ? __tiny__SelectTokenMode() : __tiny__Content__DeviceTokenMode_NoAccessToken());
										})
									);
									
									WF_DeviceOAuth.StartWorkflow("refresh-device-access-token",WF_StateObject_DeviceOAuth);
									FSM_Content.Goto(__tiny__Content__Waiting());
								}
							}
							
							if(GUILayout.Button("Clear"))
							{
								WF_StateObject_DeviceOAuth.SetResultCallbacks();
								WF_DeviceOAuth.StartWorkflow("clear-device-access-token",WF_StateObject_DeviceOAuth);
								WF_DeviceOAuth.Stop();
								FSM.Goto(__tiny__SelectTokenMode());
							}
							
							EditorGUILayout.EndHorizontal();
						}
					}
					
					Repaint();
				});
			}
			
			ENTER_STATE:
			{
				// refresh our state object with new datas
				WF_StateObject_DeviceOAuth = new workflows.OAuthDeviceAccessTokenWorkflowStateObject();
				
				if(WF_StateObject_DeviceOAuth.ClientInformation.HasEmptyFields)
					foldout_clientInfo = true;
				
				if(WF_StateObject_DeviceOAuth.CachedToken==null)
				{
					if(!WF_StateObject_DeviceOAuth.ClientInformation.HasEmptyFields)
						foldout_tokenInfo = true;
				}
				else
				{
					foldout_tokenInfo = true;
				
					WF_StateObject_DeviceOAuth.SetResultCallbacks(
						new System.Action<bool> ((success) =>
					    {
							WF_DeviceOAuth.Stop();
						})
					);
					
					WF_DeviceOAuth.StartWorkflow("validate-device-access-token",WF_StateObject_DeviceOAuth);
				}
				
				_contentFsmIsBusy = false;
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				
				yield return null;
			}
			
			EXIT_STATE:
			{
				
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		IEnumerator __tiny__Content__Waiting()
		{
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					EditorGUILayout.HelpBox("Wait...",MessageType.Info);
				});
			}
			
			ENTER_STATE:
			{	
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		
		IEnumerator __tiny__Content__DeviceTokenMode_HaveUserCode()
		{
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					if(WF_StateObject_DeviceOAuth.UserCode.IsExpired)
						EditorGUILayout.HelpBox("User code has expired!",MessageType.Warning);
					else
						EditorGUILayout.HelpBox("Click on Allow and paste the user code, or Cancel this operation.",MessageType.Info);
						
					EditorGUILayout.BeginVertical();
					
					EditorGUILayout.TextField("User Code:",WF_StateObject_DeviceOAuth.UserCode.user_code);
					EditorGUILayout.TextField("Verification Url:",WF_StateObject_DeviceOAuth.UserCode.verification_url);
					
					EditorGUILayout.BeginHorizontal();
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Cancel"))
						{
							WF_DeviceOAuth.Stop();
							FSM.Goto(__tiny__SelectTokenMode());
						}
					}
					
					if(GUILayout.Button("Allow"))
					{
						EditorGUIUtility.systemCopyBuffer = WF_StateObject_DeviceOAuth.UserCode.user_code;
						Application.OpenURL(WF_StateObject_DeviceOAuth.UserCode.verification_url);
					}
					
					EditorGUILayout.EndHorizontal();
					
					if(!WF_StateObject_DeviceOAuth.UserCode.IsExpired)
						EditorGUILayout.LabelField("User code will expire in: " + WF_StateObject_DeviceOAuth.UserCode.ExpiresInSeconds.ToString() + " seconds");

					EditorGUILayout.EndVertical();
					Repaint();
				});
			}
			
			ENTER_STATE:
			{	
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		
		IEnumerator __tiny__Content__DeviceTokenMode_NoUserCode()
		{
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					EditorGUILayout.HelpBox(@"Failed to retrieve user code.  Verify your client information.  Make sure that the OAuth Client ID you are using is of a Installed Application\Other type.",MessageType.Error);
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Retry"))
						{
							WF_DeviceOAuth.Stop();
							FSM.Goto(__tiny__SelectTokenMode());
						}
					}
				});
			}
			
			ENTER_STATE:
			{	
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		
		IEnumerator __tiny__Content__DeviceTokenMode_NoAccessToken()
		{
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					EditorGUILayout.HelpBox(@"Failed to retrieve or refresh access token.  Check your logs.",MessageType.Error);
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Retry"))
						{
							FSM.Goto(__tiny__SelectTokenMode());
						}
					}
				});
			}
			
			ENTER_STATE:
			{
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
		
		
		IEnumerator __tiny__Content__ServiceTokenMode_NoAccessToken()
		{
			DEFINE_GUI:
			{
				FSM_Content.SetGui(() => {
					EditorGUILayout.HelpBox(@"Failed to retrieve access token.  Check your logs and make sure your certificate is valid.",MessageType.Error);
					
					if(FSM.CanTransition)
					{
						if(GUILayout.Button("Retry"))
						{
							FSM.Goto(__tiny__SelectTokenMode());
						}
					}
				});
			}
			
			ENTER_STATE:
			{
			}
			
			UPDATE_STATE:
			while(!FSM_Content.NextStateRequested)
			{
				yield return null;
			}
			
			EXIT_STATE:
			{
			}
			
			FSM_Content.CurrentStateCompleted();
			yield return null;
		}
	}
}