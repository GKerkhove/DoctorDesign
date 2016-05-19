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
	public class MyJailBaseTestSources : MonoBehaviour
	{
		private SourceRecord[] sourceRecords;

		IEnumerator Start ()
		{
			yield return new WaitForSeconds(5f);
		
			Action<operations.JailBaseSources, HttpResponse> onSuccess =
			((operation, response) => {

					Debug.Log ("Status: " +  operation.Response.status);
					sourceRecords = operation.Response.records;

					foreach (SourceRecord SR in sourceRecords)
					{
						Debug.Log ("Source: " + SR.name);
					}
			});

			//get all sources
			new operations.JailBaseSources().Send (onSuccess, null, null);
		
			yield break;
		}	
	}
}