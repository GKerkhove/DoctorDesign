using UnityEngine;
using System;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis.google.mappers
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpBearerAuthorizationAttribute : HttpHeaderAttribute
	{
		public HttpBearerAuthorizationAttribute(): base(MappingDirection.REQUEST,"Authorization")
		{
			Value = "Bearer invalid_token";
			
			models.OAuthConfiguration oAuthConfig = models.OAuthConfiguration.Load();
		
			if(oAuthConfig.AuthorizationType==GoogleAuthorizationType.DEVICE)
			{
				models.OAuthDeviceAccessToken tokenCache = models.OAuthDeviceAccessToken.Load();
				
				if(tokenCache!=null)
					Value = tokenCache.TokenType + " " + tokenCache.AccessToken;
			}
			else if(oAuthConfig.AuthorizationType==GoogleAuthorizationType.SERVICE)
			{
				models.OAuthJwtAccessToken tokenCache = models.OAuthJwtAccessToken.Load();
				
				if(tokenCache!=null)
					Value = tokenCache.TokenType + " " + tokenCache.AccessToken;
			}
			else
				Configuration.Log("(Google.HttpBearerAuthorization) Http Bearer Authorization attribute could not determine OAuth flow type!", LogSeverity.WARNING);
	
		}
	}
}
