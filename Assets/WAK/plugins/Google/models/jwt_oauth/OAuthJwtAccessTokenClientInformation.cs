using UnityEngine;

using System;
using System.Text;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hg.ApiWebKit.apis.google.models
{ 
	public class OAuthJwtAccessTokenMissingClientInformation: OAuthJwtAccessTokenClientInformation
	{
	
	}

	[Serializable]
	public class OAuthJwtAccessTokenClientInformation
	{
		//**** begin serializer
		public string ClientIdEmail = "";
		public string Audience = "https://accounts.google.com/o/oauth2/token";
		public string Scope = "";
		public string CertificateResourceName = "";
		//**** end serializer
		
		public bool HasEmptyFields
		{
			get
			{
				return
					ClientIdEmail.Trim().Length == 0 ||
					Audience.Trim().Length == 0 ||
					Scope.Trim().Length == 0 ||
					CertificateResourceName.Trim().Length == 0;
			}
		}
		
		public static OAuthJwtAccessTokenClientInformation Load()
		{
			string cachedTokenClientInfoJson = null;

			#if UNITY_EDITOR
			cachedTokenClientInfoJson = EditorPrefs.GetString(typeof(OAuthJwtAccessTokenClientInformation).FullName,null);
			#else
			cachedTokenClientInfoJson = PlayerPrefs.GetString(typeof(OAuthJwtAccessTokenClientInformation).FullName,null);
			#endif

			OAuthJwtAccessTokenClientInformation tokenClientInformation = new OAuthJwtAccessTokenClientInformation();

			try { tokenClientInformation = hg.LitJson.JsonMapper.ToObject<OAuthJwtAccessTokenClientInformation>(cachedTokenClientInfoJson); }
			catch { }
			
			return tokenClientInformation;
		}

		public static bool Save(OAuthJwtAccessTokenClientInformation clientInformation)
		{
			string cachedTokenClientInfoJson = null;
			bool success = true;
			try
			{
				cachedTokenClientInfoJson = hg.LitJson.JsonMapper.ToJson(clientInformation);

				#if UNITY_EDITOR
				EditorPrefs.SetString(typeof(OAuthJwtAccessTokenClientInformation).FullName, cachedTokenClientInfoJson);
				#else
				PlayerPrefs.SetString(typeof(OAuthJwtAccessTokenClientInformation).FullName, cachedTokenClientInfoJson);
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
			EditorPrefs.DeleteKey(typeof(OAuthJwtAccessTokenClientInformation).FullName);
			#else
			PlayerPrefs.DeleteKey(typeof(OAuthJwtAccessTokenClientInformation).FullName);
			#endif
		}
	}
}

