using UnityEngine;
using System.Collections;
using System;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.randomuserme.models;

namespace hg.ApiWebKit.apis.randomuserme
{
	public class TestRandomUserMe : MonoBehaviour
	{
		int i = 0;
		private  Result[] result;

		IEnumerator Start ()
		{
			Configuration.SetSetting ("log-VERBOSE", true);

			Action<operations.GetRandomUserMe, HttpResponse> onSuccess =
				((operation, response) => 
				{
					result = operation.Response.results;

					foreach (Result R in result)
					{
						Debug.Log ("Generated Name: " + result[i].user.name.first + " " + result[i].user.name.last);
						i++;
					}
				});

			//Get one user
			//new operations.GetRandomUserMe ().Send (onSuccess, null, null);

			//Get 20 users, all males
			//if gender segment is removed it will return both genders, valid: male / female
			new operations.GetRandomUserMe() { 
				results = 20,  gender = "male" 
			}
			.Send (onSuccess, null, null);

			yield break;
		}
	}
}
