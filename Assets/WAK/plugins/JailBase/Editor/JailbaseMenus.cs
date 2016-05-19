using UnityEngine;
using UnityEditor;
using System.Collections;

namespace hg.ApiWebKit.apis.jailbase.editor
{
	public class JailbaseMenus
	{
		[MenuItem("Tools/Web API Kit/JailBase (Plugin)/Demo Scene")]
		static void DemoScene()
		{
			EditorApplication.OpenScene("Assets/WAK/plugins/JailBase/ExampleJailBase.unity");
		}


	}
}

