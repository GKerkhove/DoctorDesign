using UnityEngine;

using System;
using System.IO;
using System.Collections;

using hg.ApiWebKit;
using System.Reflection;

namespace hg.ApiWebKit.apis.example.media
{
	public class GetImageTest : MonoBehaviour
	{
		public bool ImageSlideshow = true;
		public Renderer RenderOnThis = null;

		public string AbsoluteUriToImage = "http://25.media.tumblr.com/b03bbd5189cf7820b1a0dfdb0ebad257/tumblr_mfxuvhEP3m1rqkb8so1_500.jpg";

		IEnumerator Start()
		{
			Debug.Log ("Waiting...");
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));
			
			Debug.Log("Download Image");

			new operations.GetImage {
				ImageUri = AbsoluteUriToImage
			}
			.Send(OnSuccess,OnFailure,OnComplete);

			yield break;
		}

		private void OnSuccess(operations.GetImage operation, core.http.HttpResponse response)
		{
			Debug.Log("Success");

			RenderOnThis.material.mainTexture = operation.ImageTexture;
		}

		private void OnFailure(operations.GetImage operation, core.http.HttpResponse response)
		{
			Debug.Log("Failed");

			Debug.Log("Faulted because: " + string.Join(" ; ", operation.FaultReasons.ToArray()));
		}

		private void OnComplete(operations.GetImage operation, core.http.HttpResponse response)
		{
			Debug.Log("Completed");
		}
	}
}
