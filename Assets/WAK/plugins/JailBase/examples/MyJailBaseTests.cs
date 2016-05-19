using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.jailbase.operations;
using hg.ApiWebKit.apis.example.media.operations;
using hg.ApiWebKit.apis.jailbase.models;


namespace hg.ApiWebKit.apis.jailbase
{
	public class MyJailBaseTests : MonoBehaviour
	{
		private Record[] records;
		private Dictionary<string,Texture2D> mugshots;
		
		int i = 0;

		IEnumerator Start ()
		{
			yield return new WaitForSeconds(5f);
		
			_emptyTexture = new Texture2D(200,200);
		
			mugshots = new Dictionary<string, Texture2D>();

			Action<operations.RecentAndSearch,HttpResponse> onSuccess =
			((operation, response) => 
			{
				records = operation.Response.records;

				foreach (Record R in records)
				{
					new GetImage { ImageUri = R.mugshot}
					.Send(new Action<GetImage,HttpResponse>((imageop, imageresponse) => 
						{
							mugshots.Add(imageop.ExtraParameters[0], imageop.ImageTexture);
						}),
						null,
						null, 
						R.id
					);
				}
			});
		
			//search method:
			//new operations.RecentAndSearch().DoSearch("il-ccso","smith"null)
			//.Send (onSuccess, OnFailure, null);
		
			//recent method:
			new operations.RecentAndSearch().GetRecent("il-ccso").Send(onSuccess, OnFailure, null);
		
			yield break;
		}

		private void OnFailure(RecentAndSearch operation, HttpResponse response)
		{
			Debug.Log ("Failed for some reason!");
		}

		private Texture2D _emptyTexture;

		public void OnGUI()
		{
			if (records != null)
			{
				i = 0;

				foreach (Record R in records)
				{	
					GUILayout.BeginVertical();
				
					GUILayout.BeginHorizontal();
				
					try
					{
						GUILayout.Label (mugshots[R.id]);
					}
					catch
					{
						GUILayout.Label(_emptyTexture);
					}
					
					GUILayout.BeginVertical();
					
					foreach(string[] j in R.details)
					{
						GUILayout.Label(j[0] + ": " + j[1]);
					}

					GUILayout.EndVertical();

					GUILayout.EndHorizontal();

					if (GUILayout.Button(R.name))
					{
						Application.OpenURL(R.more_info_url);
					}

					GUILayout.EndVertical();
					
					i++;
				}
			}
		}
	}
}