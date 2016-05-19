using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis
{
	public class ApiWebKitInitialize : MonoBehaviour
	{
		public bool DestroyOperationOnCompletion = true;
		public float YieldTime = 0f;

		public virtual void Awake ()
		{
			Configuration.SetSetting("log-VERBOSE", true);
			Configuration.SetSetting("log-INFO", true);
			Configuration.SetSetting("log-WARNING", true);
			Configuration.SetSetting("log-ERROR", true);
			//Configuration.SetSetting("log-internal", true);

			/*Configuration.SetSetting("log-callback", 
				new Action<string, LogSeverity>((message, severity) => {
					Debug.Log("(callback) " + severity + " / " + message);
				}));*/

			Configuration.SetSetting("destroy-operation-on-completion", DestroyOperationOnCompletion);
			//Configuration.SetSetting("persistent-game-object-name", "haptixgames.com/WebApiKit");
#if UNITY_5_3
			Configuration.SetSetting("default-http-client", typeof(hg.ApiWebKit.providers.HttpUnityWebRequestClient));
#else
			Configuration.SetSetting("default-http-client", typeof(hg.ApiWebKit.providers.HttpWWWClient));
#endif
			Configuration.SetSetting("request-timeout", 10f);

			/*Configuration.SetSetting("on-http-start", 
				new Action<HttpRequest>((request) => { 
					Debug.LogWarning("(on-http-start) id:" + ((HttpRequest)request).RequestModelResult.TransactionId);
				}));

			Configuration.SetSetting("on-http-finish", 
				new Action<HttpResponse>((response) => { 
					Debug.LogWarning("(on-http-finish) id:" + ((HttpResponse)response).Request.RequestModelResult.TransactionId);
				}));*/

			Configuration.SetSetting("yield-time", YieldTime);
		}

		public virtual void Start()
		{
			Configuration.Bootstrap();
		}
	}
}
