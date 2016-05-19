using UnityEngine;

using System;
using System.IO;
using System.Collections;

using hg.ApiWebKit;
using System.Reflection;

namespace hg.ApiWebKit.apis.example.assemblies
{
	public class ListResourcesInAssembly : MonoBehaviour
	{
		public bool ImageSlideshow = true;
		public Renderer RenderOnThis = null;

		public string AbsoluteUriToAssembly = "http://unity3dassets.com/demos/wak/AssemblyBundleDemo.dll";

		IEnumerator Start()
		{
			Debug.Log ("Waiting...");
			yield return new WaitForSeconds(Configuration.GetSetting<float>("yield-time",0f));
			
			Debug.Log("Download Assembly");

			new operations.GetAssembly {
				AssemblyUri = AbsoluteUriToAssembly
			}
			.Send(OnSuccess,OnFailure,OnComplete);

			yield break;
		}

		private void OnSuccess(operations.GetAssembly operation, core.http.HttpResponse response)
		{
			Debug.Log("Success");

			StartCoroutine(slideShow(operation.ExternalAssembly));
		}

		private void OnFailure(operations.GetAssembly operation, core.http.HttpResponse response)
		{
			Debug.Log("Failed");
		}

		private void OnComplete(operations.GetAssembly operation, core.http.HttpResponse response)
		{
			Debug.Log("Completed");
		}

		IEnumerator slideShow(Assembly assy)
		{
			foreach(string resourceName in assy.GetManifestResourceNames())
			{
				Debug.Log ("RESOURCE - " + assy.ToString() + " :: " + resourceName);

				if( (resourceName.ToLower().EndsWith("jpg") || resourceName.ToLower().EndsWith("png")) && ImageSlideshow && RenderOnThis)
				{
					Texture2D t = new Texture2D(1,1);
					t.LoadImage(GetByteResource(resourceName,assy));
					RenderOnThis.material.mainTexture = t;

					yield return new WaitForSeconds(3.0f);
				}
			}

			yield break;
		}

		private Stream GetResourceStream (string resourceName, Assembly assembly)
		{
			if (assembly == null)
			{
				assembly = Assembly.GetExecutingAssembly ();
			}
			
			return assembly.GetManifestResourceStream (resourceName);
		}

		private byte[] GetByteResource (string resourceName, Assembly assembly)
		{
			Stream byteStream = GetResourceStream (resourceName, assembly);
			byte[] buffer = new byte[byteStream.Length];
			byteStream.Read (buffer, 0, (int)byteStream.Length);
			byteStream.Close ();
			
			return buffer;
		}
	}
}
