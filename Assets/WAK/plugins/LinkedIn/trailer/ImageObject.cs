using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;

namespace hg.ApiWebKit.apis.linkedin.trailer
{
	public class ImageObject : MonoBehaviour
	{
		private behaviors.LinkedInRetrieveImageBehavior _imageBehavior;
		private Renderer _assignImageTo;

		void Awake()
		{
			_imageBehavior = this.gameObject.AddComponent<behaviors.LinkedInRetrieveImageBehavior>();
			_assignImageTo = this.gameObject.GetComponent<Renderer>();
		}

		public void FetchImage(string uri)
		{
			StartCoroutine(fetchImage(uri));
		}

		private IEnumerator fetchImage(string uri)
		{
			_imageBehavior.AbsoluteUri = uri;
			yield return StartCoroutine(_imageBehavior.ExecuteAndWait());
			if(_imageBehavior.Status == ApiBehaviorStatus.SUCCESS)
				_assignImageTo.material.mainTexture = _imageBehavior.Image;

			yield break;
		}
	}
}
