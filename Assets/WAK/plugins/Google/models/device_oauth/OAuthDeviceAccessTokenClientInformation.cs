using UnityEngine;

using System;
using System.Text;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hg.ApiWebKit.apis.google.models
{ 
	public class OAuthDeviceAccessTokenMissingClientInformation: OAuthDeviceAccessTokenClientInformation
	{
	
	}

	[Serializable]
	public class OAuthDeviceAccessTokenClientInformation
	{
		//**** begin serializer
		public string ClientId = "";
		public string Secret = "";
		public string Scope = "";
		//**** end serializer
		
		public bool HasEmptyFields
		{
			get
			{
				return
					ClientId.Trim().Length == 0 ||
					Secret.Trim().Length == 0 ||
					Scope.Trim().Length == 0;
			}
		}
		
		public static OAuthDeviceAccessTokenClientInformation Load()
		{
			string cachedTokenClientInfoJson = null;

			#if UNITY_EDITOR
			cachedTokenClientInfoJson = EditorPrefs.GetString(typeof(OAuthDeviceAccessTokenClientInformation).FullName,null);
			#else
			cachedTokenClientInfoJson = PlayerPrefs.GetString(typeof(OAuthDeviceAccessTokenClientInformation).FullName,null);
			#endif

			OAuthDeviceAccessTokenClientInformation tokenClientInformation = new OAuthDeviceAccessTokenClientInformation();

			try { tokenClientInformation = hg.LitJson.JsonMapper.ToObject<OAuthDeviceAccessTokenClientInformation>(cachedTokenClientInfoJson); }
			catch { }

			return tokenClientInformation;
		}

		public static bool Save(OAuthDeviceAccessTokenClientInformation clientInformation)
		{
			string cachedTokenClientInfoJson = null;
			bool success = true;
			try
			{
				cachedTokenClientInfoJson = hg.LitJson.JsonMapper.ToJson(clientInformation);

				#if UNITY_EDITOR
				EditorPrefs.SetString(typeof(OAuthDeviceAccessTokenClientInformation).FullName, cachedTokenClientInfoJson);
				#else
				PlayerPrefs.SetString(typeof(OAuthDeviceAccessTokenClientInformation).FullName, cachedTokenClientInfoJson);
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
			EditorPrefs.DeleteKey(typeof(OAuthDeviceAccessTokenClientInformation).FullName);
			#else
			PlayerPrefs.DeleteKey(typeof(OAuthDeviceAccessTokenClientInformation).FullName);
			#endif
		}
		
		//TODO: provide an editor list of scopes
		/*{
			get
			{
				return string.Join(" ", _scope.ToArray());
			}

			set
			{
				string[] values = value.Split(new string[] { " " },StringSplitOptions.RemoveEmptyEntries);

				foreach(string v in values)
				{
					AddScope(v);
				}
			}
		}*/
		
		/*
		public List<string> _scope = new List<string>();

		public List<string> Scopes
		{
			get 
			{ 
				return _scope;
			}
		}

		public void AddScope(string scope)
		{
			if(_scope.Contains(scope))
				return;

			_scope.Add(scope);
		}

		public void RemoveScope(string scope)
		{
			_scope.Remove(scope);
		}*/
	}
}

