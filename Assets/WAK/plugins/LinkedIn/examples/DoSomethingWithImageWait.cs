using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class DoSomethingWithImageWait: MonoBehaviour
	{
		public behaviors.LinkedInRetrieveImageBehavior RetrieveImageBehavior;

		public string AbsoluteUri = null;

		[SerializeField]
		private Texture2D _image;

		IEnumerator Start()
		{
			Configuration.Log("RetrieveImage Starting...", LogSeverity.VERBOSE);
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));

			RetrieveImageBehavior.AbsoluteUri = AbsoluteUri;
			yield return StartCoroutine(RetrieveImageBehavior.ExecuteAndWait());
			_image = RetrieveImageBehavior.Image;

			Configuration.Log("RetrieveImage call is done with status : " + RetrieveImageBehavior.Status, LogSeverity.VERBOSE);
			Configuration.Log("RetrieveImage " + _image, LogSeverity.VERBOSE); 

			yield break;
		}
	}
}
