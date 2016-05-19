using UnityEngine;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;
using hg.ApiWebKit.apis.linkedin.operations;


namespace hg.ApiWebKit.apis.linkedin.operations
{
	[HttpGET] 
	[HttpPath(null,"{$absoluteUrl}")]
	[HttpContentType("text/html")]
	[HttpAccept("image/*")]
#if UNITY_IOS
	[HttpFaultNon2XX]
#else
	[HttpFaultNon200]
#endif
	[HttpFaultInvalidMediaType]
	public class GetImage : LinkedInOperation
	{
		[HttpUriSegment]
		public string absoluteUrl;

		[HttpResponseTexture2DBody]
		public Texture2D ImageTexture;

		protected override HttpRequest ToRequest (params string[] parameters)
		{
			return base.ToRequest (parameters);
		}

		protected override void OnRequestComplete (HttpResponse response)
		{
			base.OnRequestComplete (response);

			//Debug.Log(this.ToString());
		}
	}
}

