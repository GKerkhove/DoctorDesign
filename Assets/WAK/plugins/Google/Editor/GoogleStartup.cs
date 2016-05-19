using UnityEngine;
using UnityEditor;
using System;
using hg.ApiWebKit.apis.haptix.editor;

namespace hg.ApiWebKit.apis.google.editor
{
	[InitializeOnLoad]
	public class GoogleStartup
	{
		static GoogleStartup()
		{
			models.OAuthConfiguration configuration = models.OAuthConfiguration.Load ();
		
			/*if(configuration.AuthorizationType==GoogleAuthorizationType.UNKNOWN && EditorApplication.timeSinceStartup < 6)
			{
				EditorApplication.delayCall += showTokenEditor;
			}
			else*/
			//{
				if(configuration.AuthorizationType==GoogleAuthorizationType.SERVICE)
				{
					models.OAuthJwtAccessTokenClientInformation cli = models.OAuthJwtAccessTokenClientInformation.Load();
					
					if(cli==null && !OAuthAccessTokenEditor.IsOpen)
					{
						EditorApplication.delayCall += showTokenEditor;
					}
				}
				else if(configuration.AuthorizationType==GoogleAuthorizationType.DEVICE)
				{
					models.OAuthDeviceAccessTokenClientInformation cli = models.OAuthDeviceAccessTokenClientInformation.Load();
					
					if(cli==null && !OAuthAccessTokenEditor.IsOpen)
					{
						EditorApplication.delayCall += showTokenEditor;
					}
				}
			//}
			 
			
			if(configuration.AuthorizationType==GoogleAuthorizationType.DEVICE)
				Configuration.SetSetting("google.oauth-consent-interceptor",new GoogleConsentInterceptorEditorImpl() as IGoogleConsentInterceptor);
		}
		
		static void showTokenEditor()
		{
			EditorApplication.delayCall -= showTokenEditor;
			
			HaptixEditorWindow.Show<OAuthAccessTokenEditor>();
		}
	}
}







/*
#if UNITY_EDITOR
public enum EditorState
{
	UNKNOWN,
	EDITOR,
	PLAYMODE
}

private EditorState _state = EditorState.UNKNOWN;
#endif

#if UNITY_EDITOR
Configuration.Log("[Google Initializer] Starting.  Assuming Editormode.", LogSeverity.VERBOSE);
Configuration.SetSetting("editor.isPlaymode",false);
_state = EditorState.EDITOR;
UnityEditor.EditorApplication.playmodeStateChanged += StateChange;
#endif

#if UNITY_EDITOR
Configuration.SetSetting("google.oauth-consent-interceptor",new GoogleConsentInterceptorEditorImpl() as IGoogleConsentInterceptor);
#else

#if UNITY_EDITOR
void StateChange()
{
	if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && UnityEditor.EditorApplication.isPlaying)
	{
		Configuration.Log("[Google Initializer] Entering Playmode.", LogSeverity.VERBOSE);
		Configuration.SetSetting("editor.isPlaymode",true);
		_state = EditorState.PLAYMODE;
		
		Configuration.SetSetting("google.oauth-consent-interceptor",OAuthConsentInterceptor as IGoogleConsentInterceptor);
	}
	else if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && UnityEditor.EditorApplication.isPlaying)
	{
		Configuration.Log("[Google Initializer] Exiting Playmode.", LogSeverity.VERBOSE);
		
		Configuration.SetSetting("google.oauth-consent-interceptor",new GoogleConsentInterceptorEditorImpl() as IGoogleConsentInterceptor);
	}
	else if(UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
	{
		Configuration.Log("[Google Initializer] Exiting Editormode.", LogSeverity.VERBOSE);
	}
	else
	{
		Configuration.Log("[Google Initializer] Entering Editormode.", LogSeverity.VERBOSE);
		Configuration.SetSetting("editor.isPlaymode",false);
		_state = EditorState.EDITOR;
	}
	
}

void OnDestroy()
{
	UnityEditor.EditorApplication.playmodeStateChanged -= StateChange;
	Configuration.Log("[Google Initializer] Stopping.");
}
#endif
*/
