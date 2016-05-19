using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Reflection;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.haptix.operations
{
	[HttpGET]
	[HttpTimeout(10f)]
	[HttpPath(null,"http://exist1.haptixgames.com:8080/exist/restxq/HostedAds/Ad")]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpFaultWhen("AdId", HttpFaultWhenCondition.Is, null)]
	[HttpFaultWhen("AdImage", HttpFaultWhenCondition.Is, null)]
	[HttpFaultNon2XX]
	public class GetHostedAd : HttpOperation
	{
		//request
		[HttpQueryString]
		public string AdType = "random";
	
		//response
		[HttpResponseHeader("X-Ad-Id")]
		public string AdId = "";
		
		[HttpResponseHeader("X-Ad-TTL")]
		public int AdTTL = 10;
	
		[HttpResponseHeader("X-Ad-Link")]
		public string AdLink = "";

		[HttpResponseTextBody(Converters= new[] { typeof(hg.ApiWebKit.converters.Base64DecodeToBytes), typeof(hg.ApiWebKit.converters.DeserializeTexture2D) })]
		public Texture2D AdImage;
		
		[HttpResponseHeader("X-Ad-Error-Code")]
		public string ErrorCode = "N/A";
		
		[HttpResponseHeader("X-Ad-Error-Description")]
		public string ErrorDescription = "N/A";
		
		public models.HostedAdvertisement HostedAd = null;
		
		protected override void FromResponse (HttpResponse response)
		{
			base.FromResponse (response);
			
			if(AdTTL>0 && !string.IsNullOrEmpty(AdId) && AdImage!=null)
			{
				HostedAd = ScriptableObject.CreateInstance<models.HostedAdvertisement>(); 
				HostedAd.Id = AdId;
				HostedAd.Image = AdImage;
				HostedAd.ImpressionDuration = AdTTL;
				HostedAd.Link = AdLink;   
			
			}
			else
				HostedAd = ScriptableObject.CreateInstance<models.MissingHostedAdvertisement>();
		}
	}
}