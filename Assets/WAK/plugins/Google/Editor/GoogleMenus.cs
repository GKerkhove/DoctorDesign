using UnityEngine;
using UnityEditor;
using System.Collections;
using hg.ApiWebKit.apis.haptix.editor;

namespace hg.ApiWebKit.apis.google.editor
{
	public class GoogleMenus
	{
		[MenuItem("Tools/Web API Kit/Google (Plugin)/API/Directory")]
		static void ApiDirectoryEditor_Show()
		{
			ApiDirectoryEditor.Init();
		}
	
		[MenuItem("Tools/Web API Kit/Google (Plugin)/API/Discovery Check")]
		static void ApiDiscoveryCheck()
		{
			Application.OpenURL("https://discovery-check.appspot.com/");
		}
	
		[MenuItem("Tools/Web API Kit/Google (Plugin)/API/Discovery Documentation")]
		static void ApiDiscoveryDocumentation()
		{
			Application.OpenURL("https://developers.google.com/discovery/");
		}
		
		
	
		[MenuItem("Tools/Web API Kit/Google (Plugin)/OAuth/Access Token")]
		static void ShowOAuthAccessTokenEditor()
		{
			HaptixEditorWindow.Show<OAuthAccessTokenEditor>();
		}

		[MenuItem("Tools/Web API Kit/Google (Plugin)/OAuth/Documentation")]
		static void OauthDocumentation()
		{
			Application.OpenURL("https://developers.google.com/accounts/docs/OAuth2");
		}

		[MenuItem("Tools/Web API Kit/Google (Plugin)/OAuth/Playground")]
		static void OauthPlayground()
		{
			Application.OpenURL("https://developers.google.com/oauthplayground/");
		}



		[MenuItem("Tools/Web API Kit/Google (Plugin)/Apigee Console")]
		static void ApigeeConsole()
		{
			Application.OpenURL("https://apigee.com/console");
		}
	}
}



