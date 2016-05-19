using UnityEngine;
using System.Collections;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.apis.randomuserme.models;

namespace hg.ApiWebKit.apis.randomuserme.operations
{
	[HttpGET]
	[HttpPath(null,"http://api.randomuser.me/")]
	[HttpTimeout(10f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpUnityWebRequestClient))]
	[HttpAccept("application/json")]
	public class GetRandomUserMe : HttpOperation
	{
		
		//?results=20 fetch up to 20 users
		//?gender=female or male
		//?seed=foobar
		//?results=20 & gender=male

		//max value is 20
		[HttpQueryString]
		public int results;

		[HttpQueryString]
		public string gender;

		[HttpQueryString]
		public string seed;
		
		[HttpResponseJsonBody()]
		public Results Response;

		protected override void FromResponse (HttpResponse response)
		{
			base.FromResponse (response);
		}
	}
}
