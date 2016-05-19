using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.core.http
{
	public class HttpRequestModel
	{
		[Serializable]
		public class HttpRequestModelResult
		{
			[HideInInspector]
			public HttpOperation Operation = null;
			public string TransactionId = null;
			public float Timeout = 0f;
			public string Verb = null;
			public string Uri = null;
			public string EscapedUri = null;
			public Dictionary<string,string> Headers = null; //TODO : serialize in inspector
			[HideInInspector]
			public byte[] Data = null;
			public Type ProviderType = null;

			public string Summary()
			{
				string h = "";

				foreach(KeyValuePair<string,string> de in Headers)
				{
					h += "\t<color=grey>Key: " + de.Key + " Value: " + de.Value + "</color>\n";
				}

				return
					"<color=white><b>HTTP Request Model Result</b></color>\n" +
						"<color=grey>Transaction-Id: </color><color=cyan>" + TransactionId + "</color>\n" +
						"<color=grey>Client-Type: </color><color=cyan>" + ProviderType.FullName + "</color>\n" +
						"<color=grey>Verb: </color><color=cyan>" + Verb + "</color>\n" +
						"<color=grey>Uri: </color><color=cyan>" + Uri + "</color>\n" +
						"<color=grey>Escaped-Uri: </color><color=cyan>" + EscapedUri + "</color>\n" +
						"<color=grey>Timeout: </color><color=cyan>" + Timeout + "</color>\n" +
						"<color=grey>Data-Length: </color><color=cyan>" + ((Data == null) ? "(null)" : Data.Length.ToString()) + "</color>\n" +
						"<color=grey>Headers: </color><color=cyan>" + Headers.Count + "</color>\n" + 
						h;
			}
			
			/*
			public void ReplaceHttpHeader(string name, string value)
			{
				if(Headers.ContainsKey(name))
				{
					Headers.Remove(name);
					Operation.Log ("(HttpRequestModelResult) ReplaceHttpHeader key:" + name + " value:" + value, LogSeverity.VERBOSE);
				}
				
				Headers.Add(name,value);
			}
			*/
		}

		private class HttpBinaryFormField
		{
			public string FieldName = null;
			public string FileName = null;
			public string MimeType = null;
			public byte[] Content = null;
		}

		public HttpRequestModel(
			HttpOperation owner, 
			HttpProviderAttribute httpClient, 
			HttpPathAttribute httpPath, 
			HttpMethodAttribute httpVerb, 
			HttpTimeoutAttribute httpTimeout, 
			HttpHeaderAttribute[] httpClassHeaders)
		{
			_owner = owner;

			_owner.Log("HttpRequestModel Initializing", LogSeverity.VERBOSE);

			_httpClient = httpClient;
			_httpPath = httpPath;
			_httpVerb = httpVerb;
			_httpTimeout = httpTimeout;

			_uriTemplates = new Hashtable();
			_formFields = new Hashtable();
			_binaryFormFields = new Dictionary<string, HttpBinaryFormField>();
			_httpHeaders = new Dictionary<string,string>();
			_queryStrings = new List<DictionaryEntry>();
			_stringBody = null;
			_binaryBody = null;

			_owner.Log("Processing class level maps.", LogSeverity.VERBOSE);

			foreach(HttpHeaderAttribute httpClassHeader in httpClassHeaders)
			{
				if(!httpClassHeader.MapOnRequest())
					continue;

				_owner.Log("Processing map '" + httpClassHeader.GetType().FullName + "'.", LogSeverity.VERBOSE);

				httpClassHeader.Initialize();

				string @name = httpClassHeader.OnRequestResolveName(_owner, null);
				
				object @value = httpClassHeader.OnRequestResolveValue(@name, _owner, null);
				
				@value = httpClassHeader.OnRequestApplyConverters(@value, _owner, null);

				if(!string.IsNullOrEmpty(@name))
				{
					AddHttpHeader(
						@name,
						(@value == null) ? "" : @value.ToString()
					);
				}
			}
		}

		private HttpOperation _owner;

		private HttpProviderAttribute _httpClient = null;
		private HttpPathAttribute _httpPath = null;
		private HttpMethodAttribute _httpVerb = null;
		private HttpTimeoutAttribute _httpTimeout = null;

		private Hashtable _uriTemplates;
		private Hashtable _formFields;
		private Dictionary<string, HttpBinaryFormField> _binaryFormFields;
		private Dictionary<string,string> _httpHeaders;
		private List<DictionaryEntry> _queryStrings;
		private string _stringBody;
		private byte[] _binaryBody;

		public void SetStringBody(string payload)
		{
			if(string.IsNullOrEmpty(payload))
			{
				_owner.Log("(HttpRequestBody) SetStringBody : payload cannot be empty.", LogSeverity.WARNING);
				return;
			}

			_stringBody = payload;
			_owner.Log("(HttpRequestBody) SetStringBody : '" + ((payload.Length < 1000) ? payload : "[TRUNCATED] " +  payload.Substring(0,999))+ "'", LogSeverity.VERBOSE);
			//TODO: verify UTF8 or ASCII is RFC
			SetBinaryBody(Encoding.UTF8.GetBytes(payload));
		}

		public void SetBinaryBody(byte[] payload)
		{
			_binaryBody = payload;

			if(payload == null || payload.Length < 1)
				_owner.Log ("(HttpRequestBody) SetBinaryBody is empty.", LogSeverity.WARNING);
			else
				_owner.Log("(HttpRequestBody) SetBinaryBody Length : " + payload.Length, LogSeverity.VERBOSE);
		}

		public void AddFormField(string name, string value)
		{
			try
			{
				_formFields.Add(name, value);
				_owner.Log ("(HttpRequestBody) AddFormField key:" + name + " value:" + value, LogSeverity.VERBOSE);

				if(string.IsNullOrEmpty(value))
					_owner.Log ("(HttpRequestBody) AddFormField value of key:" + name + " is empty.", LogSeverity.WARNING);
			}
			catch (Exception ex)
			{
				_owner.Log ("(HttpRequestBody) AddFormField FAILED key:" + name + " value:" + value, LogSeverity.WARNING);
			}
		}

		public void AddBinaryFormField(string name, string fileName, string mimeType, byte[] content)
		{
			try
			{
				_binaryFormFields.Add (name, new HttpBinaryFormField { 
					FieldName = name,
					FileName = fileName,
					MimeType = mimeType,
					Content = content
				});

				_owner.Log ("(HttpRequestBody) AddBinaryFormField key:" + name + " size:" + content.Length, LogSeverity.VERBOSE);
				
				if(content == null || content.Length < 1)
					_owner.Log ("(HttpRequestBody) AddBinaryFormField content of key:" + name + " is empty.", LogSeverity.WARNING);
			}
			catch (Exception ex)
			{
				_owner.Log ("(HttpRequestBody) AddBinaryFormField FAILED key:" + name + " size:" + content.Length, LogSeverity.WARNING);
			}
		}

		public void AddHttpHeader(string name, string value)
		{
			try
			{
				_httpHeaders.Add(name, value);
				_owner.Log ("(HttpRequestBody) AddHttpHeader key:" + name + " value:" + value, LogSeverity.VERBOSE);

				if(string.IsNullOrEmpty(value))
					_owner.Log ("(HttpRequestBody) AddHttpHeader value of key:" + name + " is empty.", LogSeverity.WARNING);
			}
			catch (Exception ex)
			{
				_owner.Log ("(HttpRequestBody) AddHttpHeader FAILED key:" + name + " value:" + value, LogSeverity.WARNING);
			}
		}
		
		public void AddQueryString(string name, string value)
		{
			try
			{
				_queryStrings.Add(new DictionaryEntry(name, value));
				_owner.Log ("(HttpRequestBody) AddQueryString key:" + name + " value:" + value, LogSeverity.VERBOSE);

				if(string.IsNullOrEmpty(value))
					_owner.Log ("(HttpRequestBody) AddQueryString value of key:" + name + " is empty.", LogSeverity.WARNING);
			}
			catch (Exception ex)
			{
				_owner.Log ("(HttpRequestBody) AddQueryString FAILED key:" + name + " value:" + value, LogSeverity.WARNING);
			}
		}

		public void AddUriTemplate(string name, string value)
		{
			try
			{
				_uriTemplates.Add(name, value);
				_owner.Log ("(HttpRequestBody) AddUriTemplate key:" + name + " value:" + value, LogSeverity.VERBOSE);

				if(string.IsNullOrEmpty(value))
					_owner.Log ("(HttpRequestBody) AddUriTemplate value of key:" + name + " is empty.", LogSeverity.WARNING);
			}
			catch (Exception ex)
			{
				_owner.Log ("(HttpRequestBody) AddUriTemplate FAILED key:" + name + " value:" + value, LogSeverity.WARNING);
			}
		}

		private string buildQueryString()
		{
			string qs = "";

			if(_queryStrings.Count > 0)
			{
				qs = "?";
				
				int i = 1;
				foreach(DictionaryEntry de in _queryStrings)
				{
					qs += (string)de.Key + "=" + (string)de.Value + ((i < _queryStrings.Count) ? "&" : "");
					i++;	
				}
			}

			return qs;
		}

		private string expandUriTemplate()
		{
			string uri = _httpPath.Uri;
			
			foreach(DictionaryEntry de in _uriTemplates)
			{
				uri = uri.Replace((string)de.Key, (string)de.Value);
			}

			return uri;
		}

		private string buildUri()
		{
			return expandUriTemplate() + buildQueryString();
		}

		private WWWForm buildForm()
		{
			WWWForm form = new WWWForm();

			foreach(KeyValuePair<string, HttpBinaryFormField> bf in _binaryFormFields)
			{
				if(bf.Value.FileName == null && bf.Value.MimeType == null)	form.AddBinaryData(bf.Value.FieldName, bf.Value.Content);
				else if(bf.Value.MimeType == null)							form.AddBinaryData(bf.Value.FieldName, bf.Value.Content, bf.Value.FileName);
				else 														form.AddBinaryData(bf.Value.FieldName, bf.Value.Content, bf.Value.FileName, bf.Value.MimeType);
			}

			foreach(DictionaryEntry de in _formFields)
			{
				form.AddField((string)de.Key, (string)de.Value);
			}

			foreach(KeyValuePair<string,string> h in form.headers)
			{
				if(!_httpHeaders.ContainsKey(h.Key))
					AddHttpHeader(h.Key,h.Value);
			}

			return form;
		}

		private byte[] buildData(WWWForm form)
		{
			if(_binaryBody == null || _binaryBody.Length == 0)
				return form.data;
			else 
				return _binaryBody;
		}

		public HttpRequestModelResult Build()
		{
			string uri = buildUri();
			WWWForm form = buildForm();
			byte[] data = buildData(form);

			return new HttpRequestModel.HttpRequestModelResult() {
				Operation = _owner,
				TransactionId = _owner.TransactionId,
				ProviderType = _httpClient.ProviderType,
				Timeout = _httpTimeout.Timeout,
				Verb = _httpVerb.Verb,
				Uri = uri,
				EscapedUri = WWW.EscapeURL(uri),
				Headers = _httpHeaders, 
				Data = data 
			};
		}
	}
}

