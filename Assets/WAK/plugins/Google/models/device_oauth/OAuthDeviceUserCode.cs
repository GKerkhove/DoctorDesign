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
	public class OAuthDeviceUserCode
	{
		public string device_code;
		public string user_code;
		public string verification_url;
		public int expires_in;
		public int interval;
		
		public OAuthDeviceUserCode()
		{
			Obtained = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff");
		}
		
		[hg.LitJson.JsonIgnore]
		public string Obtained;
		
		[hg.LitJson.JsonIgnore]
		public int ExpiresInSeconds
		{
			get
			{
				try
				{
					DateTime expirationTime = DateTime.Parse(Obtained).AddSeconds(expires_in);
					return (int) ((expirationTime - DateTime.UtcNow).TotalSeconds);
				}
				catch
				{
					return 0;
				}
			}
		}
		
		[hg.LitJson.JsonIgnore]
		public bool IsExpired
		{
			get
			{
				try
				{
					DateTime expirationTime = DateTime.Parse(Obtained).AddSeconds(expires_in);
					return expirationTime < DateTime.UtcNow;
				}
				catch
				{
					return true;
				}
			}
		}
	}
}

