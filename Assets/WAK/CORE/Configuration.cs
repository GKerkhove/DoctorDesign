using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit
{
	/* The configuration class is an in-memory storage area used by the framework.
	 * 
	 */ 
	public static partial class Configuration
	{
		private static Dictionary<string, object> settings = new Dictionary<string, object>()
		{
			/*** USER KEYS ***/

			/* default http client type */
#if UNITY_5_3
			{ "default-http-client"			,		typeof(hg.ApiWebKit.providers.HttpUnityWebRequestClient) },
#else
			{ "default-http-client"			,		typeof(hg.ApiWebKit.providers.HttpWWWClient) },
#endif

			/* default request timeout in seconds */
			{ "request-timeout"				,		10f },
			{ "extend-timeout-on-transfer"	,		true },

			/*** SYSTEM KEYS ***/

			{ "destroy-operation-on-completion", 	true },					// destroy provider on trx completion

			/* system keys (user can modify the value) */
			{ "log-WARNING"					, 		true },					// log warnings
			{ "log-ERROR"					, 		true },					// log errors
			{ "log-INFO"					, 		true },					// log informational
			{ "log-VERBOSE"					, 		false },				// log verbose
			{ "log-internal"				, 		true },					// log 'hg.ApiWebKit' internal logging events
			{ "log-callback"				,		new Action<string, LogSeverity>((message, severity) => { }) },
		
			/* name of persistent 'nosql' game object */
			{ "persistent-game-object-name"	,		"unity3dassets.com" },
			#if STRIPPED
			{ "persistent-game-object-flags",		HideFlags.HideInHierarchy },
			#else
			{ "persistent-game-object-flags",		HideFlags.None },
			#endif 
			
			/* tiny-fsm settings */
			{ "tiny-fsm-game-object-flags",			HideFlags.HideAndDontSave },
			
			/* callbacks for all http activity */
			{ "on-http-start"				,		new Action<HttpRequest>((request) => { }) },
			{ "on-http-finish"				,		new Action<HttpResponse>((response) => { }) }
		};

		#region Base-Uris

		private static Dictionary<string, string> baseUris = new Dictionary<string, string>()
		{
			{ "default", "http://your.server.com/api" }			//"default" key must exist.
		};

		public static void SetDefaultBaseUri(string uri)
		{
			if(!baseUris.ContainsKey("default"))
				baseUris.Add ("default", uri);
			else
				baseUris["default"] = uri;
			
			LogInternal("default base-uri set : " + uri , LogSeverity.INFO);
		}

		public static void SetBaseUri(string name, string uri)
		{
			if(!baseUris.ContainsKey(name))
				baseUris.Add (name, uri);
			else
				baseUris[name] = uri;
			
			LogInternal(name + " base-uri set : " + uri , LogSeverity.INFO);
		}

		public static string GetBaseUri(string name)
		{
			if(name==null)
				return null;

			if(baseUris.ContainsKey(name))
				return validate_base_uri(baseUris[name]);
			else
				LogInternal(name + " does not exist in the base-uri dictionary", LogSeverity.ERROR);
			
			return null;
		}
		
		private static string validate_base_uri(string base_uri)
		{
			if(base_uri.EndsWith("/"))
			{
				LogInternal(base_uri + " base-uri must not end with '/'", LogSeverity.WARNING);
				return base_uri.Remove(base_uri.Length-1, 1);
			}
			
			return base_uri;
		}

		#endregion

		#region Settings-Dictionary

		public static bool HasSetting(string name)
		{
			return settings.ContainsKey(name);	
		}
		
		public static T GetSetting<T>(string name)
		{
			if(settings.ContainsKey(name))
				return (T)settings[name];
			else
			{
				LogInternal("Configuration does NOT contain requested key : " + name, LogSeverity.WARNING);
				return default(T);
			}
		}

		public static T GetSetting<T>(string name, T defaultValue)
		{
			if(settings.ContainsKey(name))
				return (T)settings[name];
			else
			{
				LogInternal("Configuration does NOT contain requested key : " + name, LogSeverity.WARNING);
				return defaultValue;
			}
		}

		public static object GetSetting(string name)
		{
			return GetSetting<object>(name);
		}

		public static void SetSetting(string name, object @value)
		{
			if(name=="log-VERBOSE" && (bool)@value == true)
				Debug.LogWarning("WARNING! (Web API Kit) Verbose logging has been turned on.  Verbose logging adversly affects performance!");


			if(settings.ContainsKey(name))
			{
				LogInternal("Configuration value set : " + name + " = " + @value, LogSeverity.VERBOSE);
				settings[name] = @value;
			}
			else
			{
				LogInternal("Configuration value added : " + name + " = " + @value, LogSeverity.VERBOSE);
				settings.Add (name, @value);
			}
		}
		
		public static bool RemoveSetting(string name)
		{
			if(settings.ContainsKey(name))
				return settings.Remove(name);
			
			return false;
		}

		#endregion

		#region Logging

		#if STRIPPED
		[System.Diagnostics.ConditionalAttribute("LOGGING_ON")]
		#endif
		public static void Log(string message, LogSeverity severity = LogSeverity.INFO)
		{
			if(GetSetting<bool>("log-"+severity.ToString()))
			{
				switch(severity)
				{
					case LogSeverity.ERROR:
						Debug.LogError("<color=red>{ERR}</color> " + message);
						break;
					case LogSeverity.WARNING:
						Debug.LogWarning("<color=orange>{WARN}</color> " + message);
						break;
					case LogSeverity.INFO:
						Debug.Log("<color=white>{INFO}</color> " + message);
						break;
					case LogSeverity.VERBOSE:
						Debug.Log("<color=grey>{VERB}</color> " + message);
						break;
				}
				
				Action<string, LogSeverity> callback = GetSetting<Action<string, LogSeverity>>("log-callback");
				if(callback!=null)
					callback(message, severity);
			}
		}
		
		#if STRIPPED
		[System.Diagnostics.ConditionalAttribute("LOGGING_ON")]
		#endif
		public static void LogInternal(string transactionId, string message, LogSeverity severity = LogSeverity.INFO)
		{
			if(GetSetting<bool>("log-internal"))
				Log ("<color=white>[" + transactionId + "]</color> " + message, severity);
		}

		#if STRIPPED
		[System.Diagnostics.ConditionalAttribute("LOGGING_ON")]
		#endif
		public static void LogInternal(string message, LogSeverity severity = LogSeverity.INFO)
		{
			if(GetSetting<bool>("log-internal"))
				Log (message, severity);
		}

		#endregion

		public static GameObject Bootstrap()
		{
			GameObject go = GameObject.Find("/" + Configuration.GetSetting<string>("persistent-game-object-name"));

			if(go==null) 
			{  
				go = new GameObject(Configuration.GetSetting<string>("persistent-game-object-name"));
				go.hideFlags = Configuration.GetSetting<HideFlags>("persistent-game-object-flags");
				UnityEngine.Object.DontDestroyOnLoad(go);
			}
			
			return go;
		}
	}
}