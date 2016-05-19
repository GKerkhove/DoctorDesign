using UnityEngine;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;


namespace hg.ApiWebKit.apis.example.media.operations
{
	[HttpGET] 
	[HttpPath(null,"{$ImageUri}")]
	[HttpContentType("text/html")]
	[HttpAccept("image/*,application/octet-stream")]
#if !UNITY_EDITOR && UNITY_IPHONE
	[HttpFaultNon2XX]
#else
	[HttpFaultNon200]
#endif
	[HttpFaultInvalidMediaType]
	[HttpTimeout(10f)]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpUniWebClient))]
	//[HttpProvider(typeof(hg.ApiWebKit.providers.HttpBestHttpClient))]
	public class GetImage : ExampleMediaOperation
	{
		[HttpUriSegment]
		public string ImageUri;

		//[HttpResponseBinaryBody(Converter=typeof(hg.ApiWebKit.converters.DeserializeTexture2D))]
		[HttpResponseTexture2DBody]
		public Texture2D ImageTexture;

		[HttpResponseSpriteBody]
		public Sprite ImageSprite;

		public string[] ExtraParameters;

		protected override HttpRequest ToRequest (params string[] parameters)
		{
			ExtraParameters = parameters;

			return base.ToRequest (parameters);
		}

		protected override void OnRequestComplete (HttpResponse response)
		{
			base.OnRequestComplete (response);

			//Configuration.Log(this.ToString(), LogSeverity.VERBOSE);
		}

		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}

