using UnityEngine;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;


namespace hg.ApiWebKit.apis.example.media.operations
{
	[HttpPOST] 
	[HttpPath(null,"http://images.com/{$Filename}")]
	[HttpContentType("image/png")]
	[HttpAccept("image/png")]
	[HttpTimeout(60f)]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	public class PostImage : ExampleMediaOperation
	{
		[HttpUriSegment]
		public string Filename;

		[HttpRequestBinaryBody(Converter=typeof(hg.ApiWebKit.converters.SerializeTexture2DToPng))]
		[HttpResponseBinaryBody(Converter=typeof(hg.ApiWebKit.converters.DeserializeTexture2D))]
		public Texture2D Image;

		protected override HttpRequest ToRequest (params string[] parameters)
		{
			return base.ToRequest (parameters);
		}

		protected override void OnRequestComplete (HttpResponse response)
		{
			base.OnRequestComplete (response);

			Debug.Log(this.ToString());
		}

		public override string ToString ()
		{
			return Utilities.InstanceToString(this);
		}
	}
}

