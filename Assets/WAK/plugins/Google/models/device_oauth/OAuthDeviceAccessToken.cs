using UnityEngine;

using System;
using System.Text;
using System.Collections;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hg.ApiWebKit.apis.google.models
{ 
	public class OAuthDeviceAccessToken
	{
		//**** begin serializer
		[hg.LitJson.JsonProperty("access_token")]
		public string AccessToken;
		[hg.LitJson.JsonProperty("token_type")]
		public string TokenType;
		[hg.LitJson.JsonProperty("expires_in")]
		public int ExpirationInterval;
		[hg.LitJson.JsonProperty("refresh_token")]
		public string RefreshToken;
		
		public string Obtained;
		//**** end serializer
		
		[hg.LitJson.JsonIgnore]
		public int ExpiresInSeconds
		{
			get
			{
				try
				{
					DateTime expirationTime = DateTime.Parse(Obtained).AddSeconds(ExpirationInterval);
					return (int) ((expirationTime - DateTime.UtcNow).TotalSeconds);
				}
				catch
				{
					return 0;
				}
			}
		}
		
		public static OAuthDeviceAccessToken Load()
		{
			string cachedTokenJson = null;

			#if UNITY_EDITOR
			cachedTokenJson = EditorPrefs.GetString(typeof(OAuthDeviceAccessToken).FullName,null);
			#else
			cachedTokenJson = PlayerPrefs.GetString(typeof(OAuthDeviceAccessToken).FullName,null);
			#endif


			OAuthDeviceAccessToken token = new OAuthDeviceAccessToken();

			try
			{
				token = hg.LitJson.JsonMapper.ToObject<OAuthDeviceAccessToken>(cachedTokenJson);
			}
			catch(Exception ex)
			{
				Debug.LogError(ex.Message);
			}

			return token;

		}

		public static bool Save(OAuthDeviceAccessToken token)
		{
			token.Obtained = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff");

			//super impose refresh
			OAuthDeviceAccessToken existingToken = OAuthDeviceAccessToken.Load();
			if(existingToken != null)
			{
				token.RefreshToken = existingToken.RefreshToken;
			}

			string cachedTokenJson = null;
			bool success = true;
			try
			{
				cachedTokenJson = hg.LitJson.JsonMapper.ToJson(token);

				#if UNITY_EDITOR
				EditorPrefs.SetString(typeof(OAuthDeviceAccessToken).FullName, cachedTokenJson);
				#else
				PlayerPrefs.SetString(typeof(OAuthDeviceAccessToken).FullName, cachedTokenJson);
				#endif
			}
			catch(Exception ex)
			{
				Debug.LogError(ex.Message);
				success = false;
			} 

			return success;
		}

		public static void Clear()
		{
			#if UNITY_EDITOR
			EditorPrefs.DeleteKey(typeof(OAuthDeviceAccessToken).FullName);
			#else
			PlayerPrefs.DeleteKey(typeof(OAuthDeviceAccessToken).FullName);
			#endif
		}
		
		/*
		[hg.LitJson.JsonIgnore]
		public bool IsExpired
		{
			get
			{
				try
				{
					DateTime expirationTime = DateTime.Parse(Obtained).AddSeconds(ExpirationInterval);
					return expirationTime < DateTime.UtcNow;
				}
				catch
				{
					return true;
				}
			}
		}
		*/
	}
}

