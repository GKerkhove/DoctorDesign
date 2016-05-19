using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.example
{
	public class ApiExamplesInitialize : ApiWebKitInitialize 
	{
		public bool LogVerbose = false;
		public bool LogInformation = true;
		public bool LogWarning = true;
		public bool LogError = true;

		public string WakApiExample = "http://wak-api.unity3dassets.com:8080/exist/restxq";

		public override void Start()
		{
			Configuration.SetSetting("log-VERBOSE", LogVerbose);
			Configuration.SetSetting("log-INFO", LogInformation);
			Configuration.SetSetting("log-WARNING", LogWarning);
			Configuration.SetSetting("log-ERROR", LogError);

			Configuration.SetBaseUri("hg.example", WakApiExample);

			base.Start();
		}
	}
}
