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
	[HttpPOST]
	[HttpTimeout(10f)]
	[HttpPath(null,"http://exist1.haptixgames.com:8080/exist/restxq/HostedAds/Ad")]
	[HttpProvider(typeof(hg.ApiWebKit.providers.HttpWWWClient))]
	[HttpFaultNon2XX]
	[HttpFaultWhen("AdId", HttpFaultWhenCondition.Is, null)]
	[HttpContentType("image/vnd.hg+png")]
	public class SubmitHostedAd : HttpOperation
	{	
		//request
		[HttpRequestHeader("X-Ad-Link")]
		public string AdLink;
		
		[HttpRequestHeader("X-Ad-Contact")]
		public string AdContact;
		
		[HttpRequestTextBody(Converters= new[] { typeof(hg.ApiWebKit.converters.SerializeTexture2DToPng),typeof(hg.ApiWebKit.converters.Base64EncodeFromBytes) })]
		public Texture2D AdImage;
	
		//response
		[HttpResponseHeader("X-Ad-Id")]
		public string AdId;
		
		[HttpResponseHeader("X-Ad-Error-Code")]
		public string ErrorCode = "N/A";
		
		[HttpResponseHeader("X-Ad-Error-Description")]
		public string ErrorDescription = "N/A";
	}
}