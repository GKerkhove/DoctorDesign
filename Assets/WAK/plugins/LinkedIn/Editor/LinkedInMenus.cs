using UnityEngine;
using UnityEditor;
using System.Collections;

namespace hg.ApiWebKit.apis.linkedin.editor
{
	public class LinkedInMenus
	{
		/*[MenuItem("Tools/Web API Kit/LinkedIn (Plugin)/Retrieve Access Token")]
		static void RetrieveAccessToken()
		{
			RetrieveAccessTokenEditor.Init();
		}*/

		[MenuItem("Tools/Web API Kit/LinkedIn (Plugin)/API Documentation")]
		static void Documentation()
		{
			Application.OpenURL("http://developer.linkedin.com/apis");
		}

		[MenuItem("Tools/Web API Kit/LinkedIn (Plugin)/Apigee Console")]
		static void ApigeeConsole()
		{
			Application.OpenURL("https://apigee.com/console/linkedin");
		}

		[MenuItem("Tools/Web API Kit/LinkedIn (Plugin)/Demo Scene")]
		static void DemoScene()
		{
			EditorApplication.OpenScene("Assets/WAK/plugins/LinkedIn/LinkedIn-Example.unity");
		}
	}
}

