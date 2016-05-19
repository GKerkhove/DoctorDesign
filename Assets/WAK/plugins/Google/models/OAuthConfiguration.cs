using UnityEngine;

using System;
using System.Text;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hg.ApiWebKit.apis.google.models
{ 
	/// <summary>
	/// Top level Google OAuth configuration settings.
	/// </summary>
	[Serializable] public class OAuthConfiguration
	{
		//**** begin serializer
		public GoogleAuthorizationType AuthorizationType = GoogleAuthorizationType.UNKNOWN;
		//**** end serializer
		
		public static OAuthConfiguration Load()
		{
			string configurationJson = null;

			#if UNITY_EDITOR
			configurationJson = EditorPrefs.GetString(typeof(OAuthConfiguration).FullName,"{}");
			#else
			configurationJson = PlayerPrefs.GetString(typeof(OAuthConfiguration).FullName,"{}");
			#endif

			OAuthConfiguration configuration = new OAuthConfiguration();

			try { configuration = hg.LitJson.JsonMapper.ToObject<OAuthConfiguration>(configurationJson); }
			catch { }

			return configuration;
		}

		public static bool Save(OAuthConfiguration configuration)
		{
			string configurationJson = null;
			bool success = true;
			try
			{
				configurationJson = hg.LitJson.JsonMapper.ToJson(configuration);

				#if UNITY_EDITOR
				EditorPrefs.SetString(typeof(OAuthConfiguration).FullName, configurationJson);
				#else
				PlayerPrefs.SetString(typeof(OAuthConfiguration).FullName, configurationJson);
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
			EditorPrefs.DeleteKey(typeof(OAuthConfiguration).FullName);
			#else
			PlayerPrefs.DeleteKey(typeof(OAuthConfiguration).FullName);
			#endif
		}
	}
}

