using UnityEngine;
using UnityEditor;
using System.Collections;
using hg.ApiWebKit.apis.haptix.editor;

namespace hg.ApiWebKit.apis.google.cloudStorage.editor
{
	public class GoogleCloudStorageMenus
	{
		[MenuItem("Tools/Web API Kit/Google (Plugin)/Cloud Storage Editor")]
		static void ApiDirectoryEditor_Show()
		{
			HaptixEditorWindow.Show<gcse>();
		}
		
		[MenuItem("Tools/Web API Kit/Google (Plugin)/Cloud Storage Demo Scene")]
		static void DemoScene()
		{
			EditorApplication.OpenScene("Assets/WAK/plugins/Google/Google-Example-1-Unity5.unity");
		}
	}
}