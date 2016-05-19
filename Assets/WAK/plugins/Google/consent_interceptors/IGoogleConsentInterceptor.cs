using UnityEngine;
using System;
using System.Collections;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.apis.google
{
	/// <summary>
	/// Interface supporting Google DEVICE OAuth flow when Google requests user consent.
	/// </summary>
	public interface IGoogleConsentInterceptor
	{
		System.Action OnMissingClientInformation { get; }
		System.Action<hg.ApiWebKit.apis.google.models.OAuthDeviceUserCode> OnUserConsentRequest2 { get; }
		System.Action<string,string> OnUserConsentRequest { get; }
		System.Action<bool> OnUserConsentComplete { get; }

		System.Action CancelConsent { get; set; }
		System.Action RetryConsent { get; set; }
	}
}
