using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.example.apiintro
{
	public class ApiIntroTests : MonoBehaviour
	{
		IEnumerator Start()
		{
			yield return new WaitForSeconds(1.0f);

			/*Action<operations.GetCarModelsV1,HttpResponse> onSuccess =
				((operation,response) => {
					Configuration.Log (string.Join(",", operation.Response.model), LogSeverity.WARNING);
					//Debug.Log (string.Join(",", operation.Response.model));
				});

			new operations.GetCarModelsV1()
				.Send(onSuccess,null,null);
			*/

			/*new operations.GetCarModelsV2Template() {
				Make = "honda"
			}.Send(null,null,null);*/

			/*new operations.GetCarModelsV2QP() {
				Make = "honda",
				SortOrder = "descending"
			}.Send(null,null,null);*/

			/*new operations.GetCarModelsV3Template() {
				Make = "mazda"
			}.Send(null,null,null);*/

			new operations.GetCarModelsV5Template() {
				Make = "honda"
			}.Send(null,null,null);

			yield break;
		}
	}
}
