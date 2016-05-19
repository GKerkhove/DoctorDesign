using UnityEngine;

using System;
using System.Text;
using System.Collections;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.google.models
{ 
	public class OAuthFault
	{
		public enum DeviceAccessTokenErrorType
		{
			none,
			unknown,
			invalid_request,
			authorization_pending,
			slow_down,
			invalid_token,
			invalid_grant
		}

		public string error;
		public string error_description;
		public string debug_info;
		public string error_uri;

		public DeviceAccessTokenErrorType ErrorType
		{
			get
			{
				if(string.IsNullOrEmpty(error))
				{
					return DeviceAccessTokenErrorType.none;
				}

				try
				{
					return (DeviceAccessTokenErrorType)Enum.Parse(typeof(DeviceAccessTokenErrorType), error);
				}
				catch
				{
					return DeviceAccessTokenErrorType.unknown;
				}
			}
		}
	}
}

