using UnityEngine;
using UnityEditor;
using System.Collections;

namespace hg.ApiWebKit.apis.zebra.editor
{
	public class ZebraMenus
	{
		[MenuItem("Tools/Web API Kit/Zebra (Plugin)/Demo Scene")]
		static void DemoScene()
		{
			EditorApplication.OpenScene("Assets/WAK/plugins/Zebra/Zebra-Example-Unity5.unity");
		}


	}
}

