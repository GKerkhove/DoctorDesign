using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.tinyfsm;
using hg.ApiWebKit.apis.google.models;

namespace hg.ApiWebKit.apis.google
{
	/// <summary>
	/// Example of an in-game Google DEVICE OAuth flow consent interceptor.
	///  The methods in this class are invoked based on the state of the current Google access token to present a 
	///  user interface that can be used to authorize a new access token request.
	/// </summary>
	public class GoogleConsentInterceptorImpl: MonoBehaviour, IGoogleConsentInterceptor
	{
		public Action OnMissingClientInformation {
			get {
				return new Action (() => {
					_fsm.Goto(__tiny__MissingClientInformation());
				});
			}
		}
	
		public Action<string, string> OnUserConsentRequest {
			get {
				return new Action<string,string> ((userCode,verificationUrl) => {
					_userCode = userCode;
					_verificationUrl = verificationUrl;
					_fsm.Goto(__tiny__ConsentRequested());
				});
			}
		}
		
		public Action<OAuthDeviceUserCode> OnUserConsentRequest2 {
			get {
				return new Action<OAuthDeviceUserCode> ((userCode) => {
					
				});
			}
		}

		public Action<bool> OnUserConsentComplete {
			get {
				return new Action<bool> ((success) => {
					_fsm.Goto((success==true) ? __tiny__AccessGranted() : __tiny__AccessDenied());
				});
			}
		}

		public Action CancelConsent { get; set; }
		public Action RetryConsent { get; set; }

		//TODO reset these after states
		string _userCode = " ";
		string _verificationUrl = " ";

		public TinyStateMachine	_fsm;

		public void Awake()
		{
			_fsm = new TinyStateMachine(null);
			_fsm.Goto(__tiny__Idle());
		}

		public void Start()
		{

		}

		public void Update()
		{
			_fsm.Update();
		}

		public void OnGUI()
		{
			_fsm.OnGUI();
		}

		const int margin = 20;

		void _windowDo(int wid)
		{
			_windowDoAction(wid);
		}

		System.Action<int> _windowDoAction = ((id) => {});

		IEnumerator __tiny__AccessDenied()
		{
			Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));

			_windowDoAction = ((id) => 
			{
				GUILayout.BeginVertical();
				
				if(_fsm.CanTransition)
				{
					if(GUILayout.Button("Retry"))
					{
						RetryConsent();
					}
				}
				
				GUILayout.EndVertical();
			});
			
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					windowRect = GUILayout.Window(666, windowRect, _windowDo, "Oops... DENIED!.");
				});
			}
			
		ENTER_STATE:
			{
				//Debug.Log ("____ enter ACCESS_DENIED");
			}
			
		UPDATE_STATE:
				while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update ACCESS_DENIED");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				//Debug.Log ("____ exit ACCESS_DENIED");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		IEnumerator __tiny__AccessGranted()
		{
			Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
			
			_windowDoAction = ((id) => 
			                               {
				GUILayout.BeginVertical();

				if(_fsm.CanTransition)
				{
					if(GUILayout.Button("OK"))
					{
						_fsm.Goto(__tiny__Idle());
					}
				}
				
				GUILayout.EndVertical();
			});
			
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					windowRect = GUILayout.Window(666, windowRect, _windowDo, "You Can Continue...");
				});
			}
			
		ENTER_STATE:
			{
				//Debug.Log ("____ enter ACCESS_GRANTED");
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update ACCESS_GRANTED");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				//Debug.Log ("____ exit ACCESS_GRANTED");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}

		IEnumerator __tiny__ConsentRejected()
		{
			Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
			
			_windowDoAction = ((id) => 
			{
				GUILayout.BeginVertical();
				
				if(_fsm.CanTransition)
				{
					if(GUILayout.Button("Retry"))
					{
						RetryConsent();
						//TODO: we could wait here for the interceptor to call us again
					}
				}
				
				GUILayout.EndVertical();
			});
			
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					windowRect = GUILayout.Window(666, windowRect, _windowDo, "Google Really Does Need Permission");
				});
			}
			
		ENTER_STATE:
			{
				//Debug.Log ("____ enter CONSENT_REJECTED");
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update CONSENT_REJECTED");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				//Debug.Log ("____ exit CONSENT_REJECTED");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}
	

		IEnumerator __tiny__ConsentRequested()
		{
			Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));

			_windowDoAction = ((id) => 
			{
				GUILayout.BeginVertical();
				
				GUILayout.TextField(_userCode);

				if(_fsm.CanTransition)
				{
					//TODO: pass user code to urL ?
					if(GUILayout.Button("Allow"))
					{
						Application.OpenURL(_verificationUrl);
						//TODO: we can switch to another state with a cancel button only
					}
					
					if(GUILayout.Button("Cancel"))
					{
						CancelConsent();
						_fsm.Goto(__tiny__ConsentRejected());
						
					}
				}
				
				GUILayout.EndVertical();
			});

		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					windowRect = GUILayout.Window(666, windowRect, _windowDo, "Google Needs Permission");
				});
			}
			
		ENTER_STATE:
			{
				//Debug.Log ("____ enter CONSENT_REQUESTED");
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update CONSENT_REQUESTED");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				//Debug.Log ("____ exit CONSENT_REQUESTED");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		IEnumerator __tiny__MissingClientInformation()
		{
			Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
			
			_windowDoAction = ((id) => 
			{
				GUILayout.BeginVertical();
				
				GUILayout.Label("Google Client Information could not be found!");
				
				GUILayout.EndVertical();
			});
			
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {
					windowRect = GUILayout.Window(666, windowRect, _windowDo, "Missing Client Information");
				});
			}
			
		ENTER_STATE:
			{
				//Debug.Log ("____ enter MISSING_CLIENT_INFO");
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update MISSING_CLIENT_INFO");
				
				yield return null;
			}
			
		EXIT_STATE:
			{
				//Debug.Log ("____ exit MISSING_CLIENT_INFO");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}


		IEnumerator __tiny__Idle()
		{
		DEFINE_GUI:
			{
				_fsm.SetGui(() => {

				});
			}
			
		ENTER_STATE:
			{
				//Debug.Log ("____ enter IDLE");
			}
			
		UPDATE_STATE:
			while(!_fsm.NextStateRequested)
			{
				////Debug.Log ("____ update IDLE");

				yield return null;
			}
			
		EXIT_STATE:
			{
				//Debug.Log ("____ exit IDLE");
			}
			
			_fsm.CurrentStateCompleted();
			yield return null;
		}
	}
}
