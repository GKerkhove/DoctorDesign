using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class AuthenticationToken : ModelBase
	{
		public float expires_in;
		public string access_token;
	}
}