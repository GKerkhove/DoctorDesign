
//DEPRECATED: Unity5 does not expose WebScriptObject anymore.
/*
using System;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

using hg.ApiWebKit.core.http;
using System.Collections.Generic;
using System.Linq;

namespace hg.ApiWebKit.apis.linkedin.editor
{
	public class RetrieveAccessTokenEditor : EditorWindow, IHasCustomMenu
	{
		private class AuthenticationModel
		{
			public AuthenticationModel()
			{
				AuthorizationCode = "";
				AuthorizationCodeRequestUri = "";
				AccessToken = "";
			}

			public string ApiKey = "";
			public string SecretKey = "";
			public string RedirectUrl = "";

			public string AuthorizationCodeRequestUri { get; set; }
			public string AuthorizationCode  { get; set; }
			public string AccessToken { get; set; }

			public bool IsValid(out string message)
			{
				if(string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(SecretKey) || string.IsNullOrEmpty(RedirectUrl))
				{
					message = "Fields can not be empty.";
					return false;
				}
				else if(RedirectUrl.ToLower().Contains("google.com"))
				{
					message = "Google.com is being used internally by this editor.  Please use another redirect URL.";
					return false;
				}
				else if(!RedirectUrl.ToLower().StartsWith("http"))
				{
					message = "Redirect URL must start with http or https.";
					return false;
				}
				else
				{
					message = "";
					return true;
				}
			}
		}

		private bool DEBUG = false;
		
		private void log(string message)
		{
			if(!DEBUG)
				return;
			
			Debug.Log("[RetrieveAccessTokenEditor] " + message);
		}


		private enum EnumFlowState
		{
			STARTING,
			USER_INFO_COLLECT,
			USER_INFO_SUBMIT,
			AUTH_CODE_REQUEST_VALIDATION,
			AUTH_CODE_REQUEST_VALIDATION_PASSED,
			AUTH_CODE_REQUEST_VALIDATION_FAILED,
			REDIRECTING_TO_LINKEDIN_DIALOG,
			ARRIVED_TO_LINKEDIN_DIALOG,
			WAITING_AT_LINKEDIN_DIALOG,
			UNEXPECTED_LEFT_LINKEDIN_DIALOG,
			AUTH_CODE_REDIRECTION_FAILED,
			AUTH_CODE_REDIRECTION_OK,
			ACC_TOKEN_WAIT,
			ACC_TOKEN_OK,
			ACC_TOKEN_FAILED
		}

		private EnumFlowState _currentFlowState = EnumFlowState.STARTING;
		private EnumFlowState _previousFlowState = EnumFlowState.STARTING;

		private EnumFlowState changeState(EnumFlowState newState)
		{
			if(_currentFlowState == newState)
				return _currentFlowState;

			log ("State Change : " + _currentFlowState + " => " + newState);

			_previousFlowState = _currentFlowState;
			_currentFlowState = newState;

			Repaint();

			return _currentFlowState;
		}

		private bool _ready = false;

		private string _windowTitle = "LinkedIn OAuth";
		private Vector2 _shrunkSize = new Vector2(450,300);
		private Vector2 _expandedSize = new Vector2(450,750);
		private int _offsetWebViewTop = 0;

		private WebView _webView;

		private string _urlStarting = "http://www.google.com/";
		private string _urlBlank = "about:blank";
		private string _urlLinkedInAllowDialog = "https://www.linkedin.com/uas/oauth2/authorization";

		public string[] _disallowedUrlsAtLinkedInAllowDialog = new string[] {
			//"https://www.linkedin.com/reg/join",
			//"https://www.linkedin.com/uas/request-password-reset?trk=uas-resetpass",
			//"http://www.linkedin.com/static?key=user_agreement&trk=hb_ft_userag",
			//"http://www.linkedin.com/static?key=privacy_policy&trk=hb_ft_priv",
			//"https://www.linkedin.com/secure/settings?userAgree=",
			//"https://help.linkedin.com/app/answers/detail/a_id/1207/loc/na/trk/uas-oauth2-auth-code-authorize/",

			"https://help.linkedin.com/app/answers/detail/a_id/1207/",
			"https://www.linkedin.com/reg/join?trk=login_reg_redirect",
			"https://www.linkedin.com/uas/request-password-reset?trk=uas-resetpass",
			"http://www.linkedin.com/legal/user-agreement",
			"http://www.linkedin.com/legal/privacy-policy"
		};

		private AuthenticationModel _model;


		public void DebuggingToggle()
		{
			DEBUG = !DEBUG;
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(new GUIContent("Debugging"), DEBUG, new GenericMenu.MenuFunction(this.DebuggingToggle));
		}



		private bool findLinkedInFrame(out Vector2 size)
		{
			log ("Looking for LinkedIn frame");

		  	WebScriptObject linkedInFrame = scriptObject.EvalJavaScript("window.document.getElementById('frame-contents');");

			if(linkedInFrame)
			{
				log ("Found 'frame-contents'");

				try
				{
					WebScriptObject width = scriptObject.EvalJavaScript("window.getComputedStyle(document.getElementById('frame-contents'), null).getPropertyValue('width')");
					WebScriptObject height = scriptObject.EvalJavaScript("window.getComputedStyle(document.getElementById('frame-contents'), null).getPropertyValue('height')");

					int wInt = 0;
					int hInt = 0;

					bool wBool = parseInt(width.ToString(), out wInt);
					bool hBool = parseInt(height.ToString(), out hInt);

					if(wBool && hBool)
					{
						size = new Vector2(wInt, hInt);
						log ("Found LinkedIn Frame : " + size);
					}
					else
					{
						size = Vector2.zero;
						log ("Found LinkedIn Frame but parsing failed. w:" + width.ToString() + ", h:" + height.ToString());
					}


					return true;
				}
				catch(System.Exception ex)
				{
					log ("Found LinkedIn Frame but failed to find size. Error: " + ex.Message);
					size = Vector2.zero;
					return true;
				}
			}

			size = Vector2.zero;
			return false;
		}


		#region URL Tracking
		private string _currentUrl = null;
		private string _previousUrl = null;

		private void trackCurrentUrl()
		{
			string contextUrl = scriptObject.EvalJavaScript("window.location");

			if(contextUrl != _currentUrl)
			{
				_previousUrl = _currentUrl;
				_currentUrl = contextUrl;

				log("URL Changed '" + _previousUrl + "' => '" + _currentUrl + "'");
			}
		}
		#endregion

		#region JS Interaction
		private WebScriptObject scriptObject
		{
			get
			{
				return _webView.windowScriptObject;
			}
		}

		private void InvokeJSMethod(string objectName, string name, params object[] args)
		{
			if (!_webView)
			{
				return;
			}
			
			WebScriptObject webScriptObject = _webView.windowScriptObject.Get(objectName);
			
			if (webScriptObject == null)
			{
				return;
			}
			
			webScriptObject.InvokeMethodArray(name, args);
		}
		#endregion

		#region WebView Callbacks
		private void OnLoadError(string frameName)
		{
			if (!_webView)
				return;

			_isOffline = true;
			
			Debug.LogError("You might be offline.  Please re-open this window.");
		}
		
		private void OnWebViewDirty()
		{
			base.Repaint();
		}
		#endregion

		#region Window Callbacks
		//private void OnFocus()
		//{
		//	if (_webView)
		//	{
		//		_webView.Focus();
		//	}
		//}

		//private void OnLostFocus()
		//{
		//	if (_webView)
		//	{
		//		_webView.UnFocus();
		//	}
		//}

		public void OnDestroy()
		{
			UnityEngine.Object.DestroyImmediate(_webView);
		}
		#endregion

		#region WebView
		private void initWebView()
		{
			if (!_webView)
			{
				_webView = ScriptableObject.CreateInstance<WebView>();
				_webView.InitWebView((int)position.width, (int)position.height - _offsetWebViewTop, false);
				_webView.hideFlags = HideFlags.HideAndDontSave;
				_webView.SetDelegateObject(this);
			}
			
			base.wantsMouseMove = true;

			goToUrl(_urlStarting);
		}
		
		private void goToUrl(string url)
		{
			if(string.IsNullOrEmpty(url))
				return;
			
			if(!url.ToLower().StartsWith("http"))
				url = "http://" + url;
			
			if(_webView)
			{
				log ("Redirecting to : " + url);
				_webView.LoadURL(url);
			}
		}
		#endregion

		#region Window
		private Vector2 expandWindow(Vector2 requestedSize)
		{
			_offsetWebViewTop = 0;

			Resolution resolution = Screen.currentResolution;

			maximized = false;

			if(requestedSize != Vector2.zero)
				_expandedSize  = new Vector2(requestedSize.x + 50, requestedSize.y + 50);

			if(resolution.height - 100 < _expandedSize.y)
			{
				Vector2 fitSize = new Vector2(450, resolution.height - 100);
				
				minSize = fitSize;
				maxSize = fitSize;

				log ("Expanding window to : " + fitSize);

				return fitSize;
			}
			else
			{
				minSize = _expandedSize;
				maxSize = _expandedSize;

				log ("Expanding window to : " + _expandedSize);

				return _expandedSize;
			}
		}
		
		private void shrinkWindow()
		{
			_offsetWebViewTop = 0;

			maximized = false;
			minSize = _shrunkSize;
			maxSize = _shrunkSize;

			log ("Shrinking window to : " + _shrunkSize);
		}

		private RetrieveAccessTokenEditor()
		{
			log("++.ctor");

			//EditorStyles.textField.wordWrap = true;
			_model = new AuthenticationModel();
			title = _windowTitle;	
			shrinkWindow();
		}
		
		public static RetrieveAccessTokenEditor Init()
		{
			RetrieveAccessTokenEditor window = EditorWindow.GetWindow<RetrieveAccessTokenEditor>();
			window.Show();

			return window;
		}
		#endregion


		void Update()
		{
			if(!_ready)
			{
				return;
			}

			switch(_currentFlowState)
			{
			case EnumFlowState.STARTING:
				if(_currentUrl.ToLower() == _urlStarting.ToLower())
					changeState(EnumFlowState.USER_INFO_COLLECT);

				break;

			case EnumFlowState.USER_INFO_COLLECT:

				break;

			case EnumFlowState.USER_INFO_SUBMIT:
				retrieveAuthCode();
				changeState(EnumFlowState.AUTH_CODE_REQUEST_VALIDATION);

				break;

			case EnumFlowState.AUTH_CODE_REQUEST_VALIDATION:
				EditorUtility.SetDirty(LinkedInProxy.Instance);

				break;

			case EnumFlowState.AUTH_CODE_REQUEST_VALIDATION_FAILED:

				break;

			case EnumFlowState.AUTH_CODE_REQUEST_VALIDATION_PASSED:
				goToUrl(_model.AuthorizationCodeRequestUri);
				changeState(EnumFlowState.REDIRECTING_TO_LINKEDIN_DIALOG);

				break;
			
			case EnumFlowState.REDIRECTING_TO_LINKEDIN_DIALOG:
				if(_currentUrl.StartsWith(_urlLinkedInAllowDialog))
					changeState(EnumFlowState.ARRIVED_TO_LINKEDIN_DIALOG);


				break;

			case EnumFlowState.ARRIVED_TO_LINKEDIN_DIALOG:
				Vector2 frameSize = Vector2.zero;
				
				if(findLinkedInFrame(out frameSize))
				{
					expandWindow(frameSize);
					changeState(EnumFlowState.WAITING_AT_LINKEDIN_DIALOG);
				}

				break;

			case EnumFlowState.WAITING_AT_LINKEDIN_DIALOG:
				if(_disallowedUrlsAtLinkedInAllowDialog.ToList().Contains(_currentUrl))
				{
					shrinkWindow();
					changeState(EnumFlowState.UNEXPECTED_LEFT_LINKEDIN_DIALOG);
				}
				else if(_currentUrl.ToLower().StartsWith(_model.RedirectUrl.ToLower()))
				{
					shrinkWindow();
					
					Dictionary<string,string> values = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase); 
					
					int qsSeparator = _currentUrl.IndexOf("?");

					if(qsSeparator > 0)
					{
						string[] nameValues = _currentUrl.Substring(qsSeparator+1).Split(new[]{"&"},StringSplitOptions.RemoveEmptyEntries);
						
						foreach(var nameValue in nameValues)
						{
							string[] temp = nameValue.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);
							
							if(temp.Length == 2)
							{
								values.Add(temp[0],temp[1]);
							}
						}
						
						if(values.ContainsKey("error"))
						{
							changeState(EnumFlowState.AUTH_CODE_REDIRECTION_FAILED);
						}
						else if(values.ContainsKey("code"))
						{
							_model.AuthorizationCode = values["code"];
							changeState(EnumFlowState.AUTH_CODE_REDIRECTION_OK);
						}
					}
					else
					{
						changeState(EnumFlowState.AUTH_CODE_REDIRECTION_FAILED);
					}
				}

				break;

			case EnumFlowState.AUTH_CODE_REDIRECTION_FAILED:

				break;

			case EnumFlowState.AUTH_CODE_REDIRECTION_OK:
				retrieveAuthToken();
				changeState(EnumFlowState.ACC_TOKEN_WAIT);

				break;

			case EnumFlowState.ACC_TOKEN_WAIT:
				EditorUtility.SetDirty(LinkedInProxy.Instance);

				break;

			case EnumFlowState.ACC_TOKEN_OK:


				break;

			case EnumFlowState.ACC_TOKEN_FAILED:


				break;
			}
		}

		private bool _isOffline = false;

		private string _message = "Make sure your redirect URL is an exact match of the redirection. (eg: http://www.google.pl versus http://google.pl.)";
		private MessageType _messageType = MessageType.Info;

		private void OnGUI()
		{
			if(!_webView)
			{
				log("Initializing WebView");

				initWebView();
				_ready = true;

				EditorGUILayout.HelpBox("Initializing...",MessageType.Info);
				return;
			}

			if(_webView)
			{
				trackCurrentUrl();
			}

			if(_isOffline)
			{
				EditorGUILayout.HelpBox("You are offline or an error occured.",MessageType.Error);
				return;
			}

			switch(_currentFlowState)
			{
				case EnumFlowState.STARTING:
					EditorGUILayout.HelpBox("Wait...",MessageType.Info);
					break;

				case EnumFlowState.USER_INFO_COLLECT:
					_webView.UnFocus();
					
					GUI.Label(new Rect(10,50,400,20), "Request an access token."); 
					
					GUI.Label(new Rect(5,80,120,20), "API Key:"); 
					_model.ApiKey = EditorGUI.TextField(new Rect(130,80, position.width-140, 20), _model.ApiKey);
					
					GUI.Label(new Rect(5,110,120,20), "Secret Key:"); 
					_model.SecretKey = EditorGUI.PasswordField(new Rect(130,110, position.width-140, 20), _model.SecretKey);
					
					GUI.Label(new Rect(5,140,120,20), "Redirect URL:"); 
					_model.RedirectUrl = EditorGUI.TextField(new Rect(130,140, position.width-140, 20), _model.RedirectUrl);

					if(GUI.Button(new Rect(0, 250, position.width, 50), "Get It"))
					{
						if(_model.IsValid(out _message))
						{
							_messageType = MessageType.None;
							changeState(EnumFlowState.USER_INFO_SUBMIT);
						}
						else
						{
							_messageType = MessageType.Error;
						}
					}
						
					EditorGUILayout.HelpBox(_message,_messageType);
					
					break;

				case EnumFlowState.AUTH_CODE_REQUEST_VALIDATION:
					EditorGUILayout.HelpBox("Validating Request...",MessageType.Info);

					break;
				case EnumFlowState.AUTH_CODE_REQUEST_VALIDATION_FAILED:
					EditorGUILayout.HelpBox("Your request failed to validate.",MessageType.Error);
					
					if(GUI.Button(new Rect(0, 250, position.width, 50), "Try Again"))
					{
						changeState(EnumFlowState.USER_INFO_COLLECT);
					}

					break;

				case EnumFlowState.REDIRECTING_TO_LINKEDIN_DIALOG:
				case EnumFlowState.ARRIVED_TO_LINKEDIN_DIALOG:
					EditorGUILayout.HelpBox("Redirecting...",MessageType.Info);

					break;

				case EnumFlowState.WAITING_AT_LINKEDIN_DIALOG:
					GUI.changed = true;
					_webView.Focus();
					Rect webViewRect = new Rect(0, _offsetWebViewTop, position.width, position.height - _offsetWebViewTop);
					_webView.DoGUI(webViewRect);

					break;
				case EnumFlowState.UNEXPECTED_LEFT_LINKEDIN_DIALOG:
					EditorGUILayout.HelpBox("You left the LinkedIn dialog.",MessageType.Error);
					_webView.UnFocus();

					break;
				case EnumFlowState.AUTH_CODE_REDIRECTION_FAILED:
					EditorGUILayout.HelpBox("Authorization Code Redirect Failed.",MessageType.Error);
					_webView.UnFocus();

					break;
				case EnumFlowState.ACC_TOKEN_WAIT:
					EditorGUILayout.HelpBox("Wait...",MessageType.Info);
					_webView.UnFocus();

					break;
				case EnumFlowState.ACC_TOKEN_FAILED:
					EditorGUILayout.HelpBox("Failed to retrieve access token.",MessageType.Error);

					break;
				case EnumFlowState.ACC_TOKEN_OK:
					EditorGUILayout.HelpBox("Your access token is available.",MessageType.Info);
					
					if(GUI.Button(new Rect(0, 250, position.width, 50), "Copy Token to Clipboard"))
					{
						EditorGUIUtility.systemCopyBuffer = _model.AccessToken;
					}

					break;
			}

			if (GUI.changed)
				Repaint ();
		}

		private bool parseInt(string text, out int number)
		{
			try
			{
				MatchCollection matches = Regex.Matches(text, @"^\d+");
				number = int.Parse(matches[0].Value);
				return true;
			}
			catch
			{
				number = 0;
				return false;
			}
		}

		#region API - Auth Code
		private HttpCallbacks<operations.GetAuthorizationCode> _authCodeCallbacks;

		private void retrieveAuthCode()
		{
			_onAuthCodeSuccess = new Action<operations.GetAuthorizationCode, HttpResponse>
				((operation, response) => 
				 { 
					_model.AuthorizationCodeRequestUri = response.Request.RequestModelResult.Uri;
					changeState(EnumFlowState.AUTH_CODE_REQUEST_VALIDATION_PASSED);
				});
			
			_onAuthCodeFailure = new Action<operations.GetAuthorizationCode, HttpResponse>
				((operation, response) => 
				 { 
					changeState(EnumFlowState.AUTH_CODE_REQUEST_VALIDATION_FAILED);
				});
			
			_authCodeCallbacks = new HttpCallbacks<operations.GetAuthorizationCode> {
				done = _onAuthCodeSuccess,
				fail = _onAuthCodeFailure
			};
			
			LinkedInProxy.Instance.ValidateAuthCodeRequest(_model.ApiKey, _model.RedirectUrl, _authCodeCallbacks);
		}
		
		private Action<operations.GetAuthorizationCode,HttpResponse> _onAuthCodeSuccess;
		private Action<operations.GetAuthorizationCode,HttpResponse> _onAuthCodeFailure;
		#endregion

		#region API - Auth Token
		private HttpCallbacks<operations.GetAuthenticationToken> _authTokenCallbacks;
		
		private void retrieveAuthToken()
		{
			_onAuthTokenSuccess = new Action<operations.GetAuthenticationToken, HttpResponse>
				((operation, response) => 
				 { 
					_model.AccessToken = operation.token.access_token;
					changeState(EnumFlowState.ACC_TOKEN_OK);
				});
			
			_onAuthTokenFailure = new Action<operations.GetAuthenticationToken, HttpResponse>
				((operation, response) => 
				 { 
					changeState(EnumFlowState.ACC_TOKEN_FAILED);
				});
			
			_authTokenCallbacks = new HttpCallbacks<operations.GetAuthenticationToken> {
				done = _onAuthTokenSuccess,
				fail = _onAuthTokenFailure
			};
			
			LinkedInProxy.Instance.GetAccessToken(_model.AuthorizationCode, _model.ApiKey, _model.SecretKey, _model.RedirectUrl, _authTokenCallbacks);
		}
		
		private Action<operations.GetAuthenticationToken,HttpResponse> _onAuthTokenSuccess;
		private Action<operations.GetAuthenticationToken,HttpResponse> _onAuthTokenFailure;
		#endregion
	}
}
*/