using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.apis.google.operations;

namespace hg.ApiWebKit.apis.google
{
	public static class GoogleUtilities
	{
		// queue the operation up with interceptor
		public static void ToGoogle<T>(
			this T request, 
			Action<T, HttpResponse> on_success = null, 
			Action<T, HttpResponse> on_failure = null, 
			Action<T, HttpResponse> on_complete = null,
			params string[] parameters) where T : GoogleOperation
		{
			models.OAuthConfiguration oAuthConfig = models.OAuthConfiguration.Load();
			
			if(oAuthConfig.AuthorizationType==GoogleAuthorizationType.DEVICE)
			{
				GoogleDeviceOAuthOperationInterceptor interceptor = Configuration.Bootstrap().GetComponent<GoogleDeviceOAuthOperationInterceptor>();
				
				// add the http-ops interceptor on first google http-op
				if(interceptor==null)
					interceptor = Configuration.Bootstrap().AddComponent<GoogleDeviceOAuthOperationInterceptor>();
				
				interceptor.Enqueue(request,on_success,on_failure,on_complete,parameters);
			}
			else if(oAuthConfig.AuthorizationType==GoogleAuthorizationType.SERVICE)
			{
				GoogleJwtOAuthOperationInterceptor interceptor = Configuration.Bootstrap().GetComponent<GoogleJwtOAuthOperationInterceptor>();
				
				// add the http-ops interceptor on first google http-op
				if(interceptor==null)
					interceptor = Configuration.Bootstrap().AddComponent<GoogleJwtOAuthOperationInterceptor>();
				
				interceptor.Enqueue(request,on_success,on_failure,on_complete,parameters);
			}
			else
				Configuration.Log("(Google.ToGoogle) Google Operation target queue cannot be determined!", LogSeverity.ERROR);
		}
	}
}
