using UnityEngine;
using UnityEditor;
using System.Collections;

namespace hg.ApiWebKit.apis.randomuserme.editor
{
	public class RandomUserMeMenus
	{
		[MenuItem("Tools/Web API Kit/RandomUserMe (Plugin)/Demo Scene")]
		static void DemoScene()
		{
			EditorApplication.OpenScene("Assets/WAK/plugins/RandomUserMe/ExampleRandomUser-Unity5.unity");
		}


	}
}

