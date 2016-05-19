using UnityEngine;
using UnityEditor;
using System.Collections;
using hg.ApiWebKit.apis.haptix.editor;

namespace hg.ApiWebKit.editor
{
	public class Menus
	{
		[MenuItem("Tools/Web API Kit/Core/Generate Models From JSON")]
		static void ModelGenny()
		{
			//TODO
			//HaptixEditorWindow.Show<hg.ApiWebKit.apis.jsonutils.editor.JsonModelingEditor>();
			
			Application.OpenURL("http://jsonutils.com");
		}

		/*
		[MenuItem("Tools/Web API Kit/Core/Download Provider/DotNet WebRequest")]
		static void UpdateDotNetWebRequestProvider()
		{
			Application.OpenURL("http://google.com");
		}
		*/

		[MenuItem("Tools/Web API Kit/Core/Download Provider/UniWeb")]
		static void UpdateUniWebProvider()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/provider/uniweb/asset-store");
		}
		
		[MenuItem("Tools/Web API Kit/Core/Download Provider/BestHTTP")]
		static void UpdateBestHttpProvider()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/provider/besthttp/asset-store");
		}
		
		[MenuItem("Tools/Web API Kit/Core/Download Provider/UnityHTTP")]
		static void UpdateUnityHttpProvider()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/provider/unityhttp/asset-store");
		}

		[MenuItem("Tools/Web API Kit/Core/Demo Scene")]
		static void DemoScene()
		{
			EditorApplication.OpenScene("Assets/WAK/CORE/examples/WAK-Example-Unity5.unity");
		}

		[MenuItem("Tools/Web API Kit/Core/Visit Asset Store")]
		static void AssetStore__Core()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/core/asset-store");
		}

		/*
		[MenuItem("Tools/Web API Kit/Bitly/Visit Asset Store")]
		static void AssetStore_Bitly()
		{
			Application.OpenURL("http://assetstore.com");
		}

		[MenuItem("Tools/Web API Kit/Instagram/Visit Asset Store")]
		static void AssetStore_Instagram()
		{
			Application.OpenURL("http://assetstore.com");
		}
		*/

		[MenuItem("Tools/Web API Kit/LinkedIn (Plugin)/Visit Asset Store")]
		static void AssetStore_LinkedIn()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/plugin/linkedin/asset-store");
		}

		/*
		[MenuItem("Tools/Web API Kit/Mailchimp/Visit Asset Store")]
		static void AssetStore_Mailchimp()
		{
			Application.OpenURL("http://assetstore.com");
		}
		*/

		[MenuItem("Tools/Web API Kit/Mandrill (Plugin)/Visit Asset Store")]
		static void AssetStore_Mandrill()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/plugin/mandrill/asset-store");
		}
		
		[MenuItem("Tools/Web API Kit/OAuth Interceptor (Extension)/Visit Asset Store")]
		static void AssetStore_OAuthInterceptor()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/extension/oauth-interceptor/asset-store");
		}

		[MenuItem("Tools/Web API Kit/MantisBT (Plugin)/API Documenatation")]
		static void Docs_MantisBT()
		{
			Application.OpenURL("http://www.futureware.biz/mantisconnect");
		}

		/*
		[MenuItem("Tools/Web API Kit/MySpace/Visit Asset Store")]
		static void AssetStore_MySpace()
		{
			Application.OpenURL("http://assetstore.com");
		}

		[MenuItem("Tools/Web API Kit/Reddit/Visit Asset Store")]
		static void AssetStore_Reddit()
		{
			Application.OpenURL("http://assetstore.com");
		}

		[MenuItem("Tools/Web API Kit/Tumblr/Visit Asset Store")]
		static void AssetStore_Tumblr()
		{
			Application.OpenURL("http://assetstore.com");
		}

		[MenuItem("Tools/Web API Kit/WeatherBug/Visit Asset Store")]
		static void AssetStore_WeatherBug()
		{
			Application.OpenURL("http://assetstore.com");
		}
		*/


		[MenuItem("Tools/Web API Kit/Documentation/Web API Kit Videos")]
		static void Documentation_All_vids()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/core/documentation/client");
		}
		
		[MenuItem("Tools/Web API Kit/Documentation/eXist-db Server Videos")]
		static void Documentation_Server_vids()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/wak/core/documentation/server");
		}
		
		[MenuItem("Tools/Web API Kit/Documentation/Online")]
		static void Documentation_All_online()
		{
			Application.OpenURL("http://www.unity3dassets.com/r/docs/wak");
		}

		/*[MenuItem("Tools/Web API Kit/Bugs and Enhancements")]
		static void IssueTracker()
		{
			Application.OpenURL("http://");
		}*/

		[MenuItem("Tools/Web API Kit/Submit Ticket")]
		static void SubmitTicket()
		{
			hg.ApiWebKit.apis.mantisbt.editor.SubmitIssueEditor.Init();
		}
		
		[MenuItem("Tools/Web API Kit/Capture Screen")]
		static void TakeScreenshot()
		{
			Application.CaptureScreenshot("ss.png");
		}

		
		[MenuItem("internal:Tools/Web API Kit/Asset Publisher/Break Apart")]
		static void Publisher_BreakApart()
		{
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.core"), new string[] {"plugins","providers"});
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.plugin.linkedin"), new string[] {"plugins"});
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.plugin.mandrill"), new string[] {"plugins"});
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.provider.uniweb"), new string[] {"providers"});
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.provider.besthttp"), new string[] {"providers"});
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.provider.unityhttp"), new string[] {"providers"});
			Publisher_createWakTree(AssetDatabase.CreateFolder("Assets", "WAK.extension.oauth-interceptor"), new string[] {"extensions"});
			
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			Publisher__moveAssets("l:wak-CORE", "Assets/WAK", "Assets/WAK.core/WAK", null);
			Publisher__moveAssets("l:WaK-Plugin-LinkedIn", "Assets/WAK", "Assets/WAK.plugin.linkedin/WAK", null);
			Publisher__moveAssets("l:WaK-Plugin-Mandrill", "Assets/WAK", "Assets/WAK.plugin.mandrill/WAK", null);
			Publisher__moveAssets("l:WaK-Provider-UniWeb", "Assets/WAK", "Assets/WAK.provider.uniweb/WAK", null);
			Publisher__moveAssets("l:WaK-Provider-BestHTTP", "Assets/WAK", "Assets/WAK.provider.besthttp/WAK", null);
			Publisher__moveAssets("l:WaK-Provider-UnityHTTP", "Assets/WAK", "Assets/WAK.provider.unityhttp/WAK", null);
			Publisher__moveAssets("l:WaK-Extension-OAuthInterceptor", "Assets/WAK", "Assets/WAK.extension.oauth-interceptor/WAK", null);
			
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		
		[MenuItem("internal:Tools/Web API Kit/Asset Publisher/Merge")]
		static void Publisher_Merge()
		{
			Publisher__moveAssets("l:wak-CORE", "Assets/WAK.core/WAK", "Assets/WAK", "Assets/WAK.core");
			Publisher__moveAssets("l:WaK-Plugin-LinkedIn", "Assets/WAK.plugin.linkedin/WAK", "Assets/WAK", "Assets/WAK.plugin.linkedin");
			Publisher__moveAssets("l:WaK-Plugin-Mandrill", "Assets/WAK.plugin.mandrill/WAK", "Assets/WAK", "Assets/WAK.plugin.mandrill");
			Publisher__moveAssets("l:WaK-Provider-UniWeb", "Assets/WAK.provider.uniweb/WAK", "Assets/WAK", "Assets/WAK.provider.uniweb");
			Publisher__moveAssets("l:WaK-Provider-BestHTTP", "Assets/WAK.provider.besthttp/WAK", "Assets/WAK", "Assets/WAK.provider.besthttp");
			Publisher__moveAssets("l:WaK-Provider-UnityHTTP", "Assets/WAK.provider.unityhttp/WAK", "Assets/WAK", "Assets/WAK.provider.unityhttp");
			Publisher__moveAssets("l:WaK-Extension-OAuthInterceptor", "Assets/WAK.extension.oauth-interceptor/WAK", "Assets/WAK", "Assets/WAK.extension.oauth-interceptor");
			
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		static void Publisher_createWakTree(string parentId, string[] subfolders)
		{
			string parentPath = AssetDatabase.GUIDToAssetPath(parentId);
			string childId = AssetDatabase.CreateFolder(parentPath, "WAK");
			string childPath = AssetDatabase.GUIDToAssetPath(childId);
			
			if(subfolders!=null)
			{
				foreach(string folder in subfolders)
				{
					AssetDatabase.CreateFolder(childPath, folder);
				}
			}
		}

		static bool Publisher__moveAssets(string label, string sourcePath, string destinationPath, string deleteAfterMove)
		{
			bool result = true;
		
			string[] guids = AssetDatabase.FindAssets (label, new string[] { sourcePath });
			
			foreach (string guid in guids) 
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				string newPath = path.Replace(sourcePath, destinationPath);
				
				string moveMessage = AssetDatabase.MoveAsset(path,newPath);
				
				if(string.IsNullOrEmpty(moveMessage))
				{
					Debug.Log ("Move '" + path + "' => '" + newPath + "' ... OK");
				}
				else
				{
					Debug.LogError ("Move '" + path + "' => '" + newPath + "' ... FAILED [" + moveMessage + "]");
					result = false;
				}
			}
			
			if(string.IsNullOrEmpty(deleteAfterMove) && result)
			{
				string[] includes = AssetDatabase.FindAssets("l:WaK-Include", new string[] { "Assets/WAK" });
				
				foreach (string include in includes) 
				{
					string path = AssetDatabase.GUIDToAssetPath(include);
					string newPath = path.Replace(sourcePath, destinationPath);
					
					if(AssetDatabase.CopyAsset(path,newPath))
					{
						Debug.Log ("Copy '" + path + "' => '" + newPath + "' ... OK");
					}
					else
					{
						Debug.LogError ("Copy '" + path + "' => '" + newPath + "' ... FAILED");
					}
				}
			}
			
			if(!string.IsNullOrEmpty(deleteAfterMove) && result)
			{
				AssetDatabase.MoveAssetToTrash(deleteAfterMove);
			}
			
			return result;
		}
		
		
		
		[MenuItem("Tools/Web API Kit/Core/Enable Verbose Logging #&=",true)]
		static bool EnableVerboseLogging_validate()
		{
			return !Configuration.GetSetting<bool>("log-VERBOSE");
		}
		
		[MenuItem("Tools/Web API Kit/Core/Enable Verbose Logging #&=")]
		static void EnableVerboseLogging()
		{
			Configuration.SetSetting("log-VERBOSE", true);
		}
		
		[MenuItem("Tools/Web API Kit/Core/Disable Verbose Logging #&-",true)]
		static bool DisableVerboseLogging_validate()
		{
			return Configuration.GetSetting<bool>("log-VERBOSE");
		}
		
		[MenuItem("Tools/Web API Kit/Core/Disable Verbose Logging #&-")]
		static void DisableVerboseLogging()
		{
			Configuration.SetSetting("log-VERBOSE", false);
		}
	}
}

