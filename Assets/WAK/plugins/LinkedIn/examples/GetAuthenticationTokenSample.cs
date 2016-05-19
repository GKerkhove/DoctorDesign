using UnityEngine;

using System;
using System.Collections;

using hg.ApiWebKit;
using hg.ApiWebKit.apis.linkedin;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.linkedin.examples
{
	public class GetAuthenticationTokenSample: MonoBehaviour
	{
		[SerializeField]
		private string _authorizationCode;

		[SerializeField]
		private string _apiKey;

		[SerializeField]
		private string _secretKey;

		[SerializeField]
		private string _redirectUrl;

		public models.AuthenticationToken Token;

		private HttpCallbacks<operations.GetAuthenticationToken> _authTokenCallbacks;

		void Start()
		{
			retrieveAuthToken();
		}

		private void retrieveAuthToken()
		{
			_onAuthTokenSuccess = new Action<operations.GetAuthenticationToken, HttpResponse>
				((operation, response) => 
				{ 
					Debug.Log ("success");
					Token = operation.token;
				});

			_onAuthTokenFailure = new Action<operations.GetAuthenticationToken, HttpResponse>
				((operation, response) => 
				 { 
					Debug.Log ("failure");
				});

			_authTokenCallbacks = new HttpCallbacks<operations.GetAuthenticationToken> {
				done = _onAuthTokenSuccess,
				fail = _onAuthTokenFailure
			};
			
			LinkedInProxy.Instance.GetAccessToken(_authorizationCode, _apiKey, _secretKey, _redirectUrl, _authTokenCallbacks);
		}

		private Action<operations.GetAuthenticationToken,HttpResponse> _onAuthTokenSuccess;
		private Action<operations.GetAuthenticationToken,HttpResponse> _onAuthTokenFailure;


	}
}
