using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.core.http
{
	[Serializable]
	public class HttpOperation
	{
		private HttpRequest _httpRequest = null;
		public HttpRequest Request { get { return _httpRequest; } }

		private FieldInfo[] _cachedFieldInfos = null;

		public string[] InParameters { get; private set; }

		public string TransactionId { get; set; }

		public HttpOperation()
		{

		}


		public void Log(string message, LogSeverity severity = LogSeverity.INFO)
		{
			Configuration.LogInternal(TransactionId, message, severity);
		}


		public bool IsFaulted;

		[SerializeField]
		public List<string> FaultReasons = new List<string>();

		public virtual void Fault(string reason)
		{
			IsFaulted = true;

			FaultReasons.Add(reason);
		}


		public virtual void CancelRequest()
		{
			if(_httpRequest != null)
				_httpRequest.Cancel();
		}
		
		protected virtual void FromResponse(HttpResponse response)
		{

			// if the request was cancelled then we DONT have a response, quit early
			if(response.Request.CompletionState == HttpRequestState.CANCELLED || 
			   response.Request.CompletionState == HttpRequestState.TIMEOUT ||
			   response.Request.CompletionState == HttpRequestState.DISCONNECTED)
			{
				Log("Request was cancelled, timed-out, or network was unavailable.  Error : " + (response.Error == null ? "(null)" : response.Error ), LogSeverity.WARNING);
				Log(response.Summary(), LogSeverity.VERBOSE);
				Log("<color=grey>Data-Text: </color><color=cyan>" + ((response.Text == null) ? "(null)" : response.Text.Replace("<", "&lt;").Replace(">", "&gt;")) + "</color>\n", LogSeverity.VERBOSE);

				return;
			}

			// --- field infos ---
			foreach(FieldInfo instanceField in _cachedFieldInfos)
			{
				Log("Processing field '" + instanceField.Name + "'.", LogSeverity.VERBOSE);

				HttpMappedValueAttribute[] httpMappings = (HttpMappedValueAttribute[])instanceField.GetCustomAttributes(typeof(HttpMappedValueAttribute), true);
				
				foreach(HttpMappedValueAttribute httpMapping in httpMappings)
				{
					if(!httpMapping.MapOnResponse())
						continue;

					Log("Processing map '" + httpMapping.GetType().FullName + "'.", LogSeverity.VERBOSE);

					httpMapping.Initialize();

					string @name = httpMapping.OnResponseResolveName(this, instanceField, response);
					
					object @value = httpMapping.OnResponseResolveValue(@name, this, instanceField, response);
					
					@value = httpMapping.OnResponseApplyConverters(@value, this, instanceField);
					
					httpMapping.OnResponseResolveModel(@value, this, instanceField);

				}
			}

			// --- fault checks ---
			HttpFaultAttribute[] httpFaultChecks = (HttpFaultAttribute[])this.GetType().GetCustomAttributes(typeof(HttpFaultAttribute), true);
			
			foreach(HttpFaultAttribute httpFaultCheck in httpFaultChecks)
			{
				httpFaultCheck.CheckFaults(this, response);
			}
			
			
			Log(response.Summary(), LogSeverity.VERBOSE);
			Log("<color=grey>Data-Text: </color><color=cyan>" + ((response.Text == null) ? "(null)" : response.Text.Replace("<", "&lt;").Replace(">", "&gt;")) + "</color>\n", LogSeverity.VERBOSE);


			return;
		}

		public HttpPathAttribute GetHttpPath()
		{
			// --- path ---
			HttpPathAttribute[] httpPaths = (HttpPathAttribute[])this.GetType().GetCustomAttributes(typeof(HttpPathAttribute),true);
			
			if(httpPaths.Length == 0)
				throw new Exception(this.GetType().Name + " class is missing [HttpPath] attribute.");
				
			return httpPaths[0];
		}

		protected virtual HttpRequest ToRequest(params string[] parameters)
		{
			InParameters = parameters;

			TransactionId = String.Format("{0:X}", Guid.NewGuid().GetHashCode());
			Log("<color=white><b>'" + this.GetType().FullName + "'</b> HTTP Operation Initializing</color>", LogSeverity.INFO);

			// --- path ---
			HttpPathAttribute httpPath = this.GetHttpPath();

			// --- verb ---
			HttpMethodAttribute[] httpVerbs = (HttpMethodAttribute[])this.GetType().GetCustomAttributes(typeof(HttpMethodAttribute),true);

			if(httpVerbs.Length == 0)
				throw new Exception(this.GetType().Name + " class is missing [HttpMethod] attribute.");
			else if(httpVerbs.Length > 1)
				Log("Multiple [HttpMethod] attributes found.  Verb " + httpVerbs[0].Verb + " will be used.", LogSeverity.ERROR);

			Log("<color=green>" + httpVerbs[0].Verb + "</color><color=cyan><b> " + httpPath.Uri + "</b></color>");

			// --- provider ---
			#if STRIPPED
			HttpProviderAttribute[] httpProviders = new HttpProviderAttribute[] { new HttpProviderAttribute(typeof(hg.ApiWebKit.providers.HttpWWWClient)) };
			#else
			HttpProviderAttribute[] httpProviders = (HttpProviderAttribute[])this.GetType().GetCustomAttributes(typeof(HttpProviderAttribute),true);
			
			if(httpProviders.Length == 0)
			{
				httpProviders = new HttpProviderAttribute[] { new HttpProviderAttribute(Configuration.GetSetting<Type>("default-http-client")) };
				Log("Missing [HttpProvider] attribute.  Provider '" + httpProviders[0].ProviderType.FullName + "' will be used.", LogSeverity.WARNING);
			}
			#endif

			// --- timeout ---
			HttpTimeoutAttribute[] httpTimeouts = (HttpTimeoutAttribute[])this.GetType().GetCustomAttributes(typeof(HttpTimeoutAttribute),true);

			if(httpTimeouts.Length == 0)
				httpTimeouts = new HttpTimeoutAttribute[] { new HttpTimeoutAttribute(Configuration.GetSetting<float>("request-timeout")) };

			Log("Request TTL : " + httpTimeouts[0].Timeout + " seconds.", LogSeverity.VERBOSE);

			// --- headers ---
			HttpHeaderAttribute[] httpHeaders = (HttpHeaderAttribute[])this.GetType().GetCustomAttributes(typeof(HttpHeaderAttribute),true);

			// --- field infos ---
			_cachedFieldInfos = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			// --- initiaize model ---
			HttpRequestModel model = new HttpRequestModel(
				this, 
				httpProviders[0],
				httpPath,
				httpVerbs[0],
				httpTimeouts[0],
				httpHeaders
			);

			foreach(FieldInfo instanceField in _cachedFieldInfos)
			{
				Log("Processing field '" + instanceField.Name + "'.", LogSeverity.VERBOSE);

				//TODO: cache attributes for response
				HttpMappedValueAttribute[] httpMappings = (HttpMappedValueAttribute[])instanceField.GetCustomAttributes(typeof(HttpMappedValueAttribute), true);

				foreach(HttpMappedValueAttribute httpMapping in httpMappings)
				{
					if(!httpMapping.MapOnRequest())
						continue;

					Log("Processing map '" + httpMapping.GetType().FullName + "'.", LogSeverity.VERBOSE);

					httpMapping.Initialize();

					string @name = httpMapping.OnRequestResolveName(this, instanceField);

					object @value = httpMapping.OnRequestResolveValue(@name, this, instanceField);

					@value = httpMapping.OnRequestApplyConverters(@value, this, instanceField);

					httpMapping.OnRequestResolveModel(@name, @value, ref model, this, instanceField);
				}
			
			}
			

			// --- auth attribute ---
			HttpAuthorizationAttribute[] httpAuths = (HttpAuthorizationAttribute[])this.GetType().GetCustomAttributes(typeof(HttpAuthorizationAttribute),true);
			
			if(httpAuths.Length > 0)
			{
				var httpMapping = httpAuths[0];
			
				Log("Processing map '" + httpMapping.GetType().FullName + "'.", LogSeverity.VERBOSE);
				
				httpMapping.Initialize();
				
				string @name = httpMapping.OnRequestResolveName(this, null);
				
				object @value = httpMapping.OnRequestResolveValue(@name, this, null);
				
				@value = httpMapping.OnRequestApplyConverters(@value, this, null);
				
				httpMapping.OnRequestResolveModel(@name, @value, ref model, this, null);
			}
				

			HttpRequestModel.HttpRequestModelResult result = model.Build();

			Log(result.Summary(), LogSeverity.VERBOSE);

			return new HttpRequest(result);
		}

		public virtual void Send(params string[] parameters)
		{
			var request = ToRequest(parameters);
			SendRequest(request);
		}
		
		protected void SendRequest(HttpRequest request)
		{
			_httpRequest = request;
			
			HttpRequest.Send(request,
				(response) => 
					{ 
						FromResponse(response); 
						OnRequestComplete(response); 
						tryUserAction("on-complete", this, response); 
					},
				(progress, elapsed, ttl) => 
					{ 
						OnTransferProgressUpdate(progress, elapsed, ttl); 
						tryUserAction("on-progress", this, progress, elapsed, ttl);
					},
				(@from, @to) => 
					{ 
						OnRequestStateChange(@from, @to); 
						tryUserAction("on-state-change", this, @from, @to);
					});
		}
		
		private void tryUserAction(string action, params object[] args)
		{
			Delegate d = this[action];
			
			if(d == null)
				return;
			
			Log("Invoking '" + action + "' User Action", LogSeverity.VERBOSE);
			
			try
			{
				d.DynamicInvoke(args);  
			}
			catch(Exception ex)
			{
				Log("'" + action + "' User Action invocation failed hard with error: " + ex.Message, LogSeverity.ERROR);
				if(ex.InnerException!=null)
					Log("'" + action + "' User Action invocation inner error: " + ex.InnerException.Message, LogSeverity.ERROR);
			}
		}
		
		public void InvokeUserAction(string actionName, params object[] args)
		{
			tryUserAction(actionName, args);
		}
		
		protected virtual void OnRequestComplete(HttpResponse response)
		{
			Log("Request Completed", LogSeverity.VERBOSE);
		}
		
		protected virtual void OnTransferProgressUpdate(float progress, float elapsed_time, float ttl)
		{
			Log("Request Progress Updated : " + progress.ToString(), LogSeverity.VERBOSE);
		}
		
		protected virtual void OnRequestStateChange(HttpRequestState @from, HttpRequestState @to)
		{
			Log("Request State Changed : " + @from + " => " + @to, LogSeverity.VERBOSE);
		}
		
		private Dictionary<string, Delegate> _userActions = new Dictionary<string, Delegate>();
		
		public Delegate this[string actionName]
		{
			get
			{
				if(_userActions.ContainsKey(actionName))
					return _userActions[actionName];
				else 
					return null;
			}
			
			set
			{
				if(_userActions.ContainsKey(actionName))
				{
					Log("Setting '" + actionName + "' User Action", LogSeverity.VERBOSE);
					_userActions[actionName] = value;
				}
				else 
				{
					//TODO: trxId is unkown at this time
					Log("Adding '" + actionName + "' User Action", LogSeverity.VERBOSE);
					_userActions.Add(actionName, value);
				}
			}
		}

		/*
		#region request/response security
		private void secure_request(ref Hashtable request_headers)
		{
			if(this.GetType().IsDefined(typeof(authenticated), true))
				inject_basic_auth(ref request_headers);
			else if(this.GetType().IsDefined(typeof(authorized), true) && Configuration.GetSetting<bool>("rest-use-session"))
				inject_cookie(ref request_headers);
		}
		
		private void inject_cookie(ref Hashtable request_headers)
		{
			string cookie = Configuration.GetSetting<string>("SET-COOKIE");
			
			if(cookie!=null)
			{
				if(request_headers.ContainsKey("SET-COOKIE"))
				{
					request_headers["COOKIE"] = cookie;
					Configuration.LogInternalUriAction(this.GetType().Name, "cookie-add injected : ", cookie, LogSeverity.WARNING);
				}
				else
				{
					request_headers.Add("COOKIE", cookie);
					Configuration.LogInternalUriAction(this.GetType().Name, "cookie-set injected : ", cookie, LogSeverity.WARNING);
				}
			}
			else
				Configuration.LogInternalUriAction(this.GetType().Name, "cookie not available", "", LogSeverity.WARNING);
		}
		
		private void inject_basic_auth(ref Hashtable request_headers)
		{
			authenticated a =  (authenticated)(this.GetType().GetCustomAttributes(typeof(authenticated), true)[0]);
			string auth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Configuration.GetSetting<string>(a.UserSource) + ":" + Configuration.GetSetting<string>(a.PasswordSource)));
			
			if(request_headers.ContainsKey("Authorization"))
			{
				request_headers["Authorization"] = Configuration.GetSetting<string>("rest-auth") + auth;
				Configuration.LogInternalUriAction(this.GetType().Name, "Authorization Header Set : ", request_headers["Authorization"].ToString(), LogSeverity.WARNING);
			}
			else
			{
				request_headers.Add("Authorization", Configuration.GetSetting<string>("rest-auth") + auth);
				Configuration.LogInternalUriAction(this.GetType().Name, "Authorization Header Added : ", request_headers["Authorization"].ToString(), LogSeverity.WARNING);
			}
		}
		
		private void cache_cookie(Dictionary<string, string> response_headers)
		{
			if(Configuration.GetSetting<bool>("rest-use-session") &&
				response_headers.ContainsKey("SET-COOKIE") && 
				response_headers["SET-COOKIE"].ToUpper().Contains("JSESSIONID"))
			{
				Configuration.LogInternalUriAction(this.GetType().Name, "cookie cached : ", response_headers["SET-COOKIE"], LogSeverity.WARNING);
				Configuration.SetSetting("SET-COOKIE", response_headers["SET-COOKIE"]));//.Replace("Path=/exist", ""));
			}
		}
		#endregion
		*/		
	}
}

