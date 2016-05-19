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
	public class OAuthJwtAccessToken
	{
		//**** begin serializer
		[hg.LitJson.JsonProperty("access_token")]
		public string AccessToken;
		[hg.LitJson.JsonProperty("token_type")]
		public string TokenType;
		[hg.LitJson.JsonProperty("expires_in")]
		public int ExpirationInterval;
		
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
		
		public static OAuthJwtAccessToken Load()
		{
			string cachedTokenJson = null;

			#if UNITY_EDITOR
			cachedTokenJson = EditorPrefs.GetString(typeof(OAuthJwtAccessToken).FullName,null);
			#else
			cachedTokenJson = PlayerPrefs.GetString(typeof(OAuthJwtAccessToken).FullName,null);
			#endif


			OAuthJwtAccessToken token = new OAuthJwtAccessToken();

			try
			{
				token = hg.LitJson.JsonMapper.ToObject<OAuthJwtAccessToken>(cachedTokenJson);
			}
			catch(Exception ex)
			{
				Debug.LogError(ex.Message);
			}

			return token;

		}

		public static bool Save(OAuthJwtAccessToken token)
		{
			token.Obtained = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff");

			string cachedTokenJson = null;
			bool success = true;
			try
			{
				cachedTokenJson = hg.LitJson.JsonMapper.ToJson(token);

				#if UNITY_EDITOR
				EditorPrefs.SetString(typeof(OAuthJwtAccessToken).FullName, cachedTokenJson);
				#else
				PlayerPrefs.SetString(typeof(OAuthJwtAccessToken).FullName, cachedTokenJson);
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
			EditorPrefs.DeleteKey(typeof(OAuthJwtAccessToken).FullName);
			#else
			PlayerPrefs.DeleteKey(typeof(OAuthJwtAccessToken).FullName);
			#endif
		}
	}
}

