using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.google
{
	/// <summary>
	/// Type of Google OAuth flow our application is using.
	/// </summary>
	public enum GoogleAuthorizationType
	{
		UNKNOWN = 0,
		SERVICE = 1,
		DEVICE = 2,
	}
}
