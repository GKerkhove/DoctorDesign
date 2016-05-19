using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Reflection;

using hg.ApiWebKit.core.attributes;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.mappers;
using hg.ApiWebKit.faulters;

namespace hg.ApiWebKit.apis.haptix.models
{
	[Serializable]
	public class MissingHostedAdvertisement: HostedAdvertisement
	{
	
	}

	[Serializable]
	public class HostedAdvertisement: ScriptableObject
	{
		public string Id = "";
		public Texture2D Image = null;
		public int ImpressionDuration = 10;
		public string Link = "";
	}

	public class AdvertisementPostModel
	{
		public bool IsPostValid
		{
			get
			{
				return IsImageValid && IsEmailValid && IsUrlValid;
			}
		}
		
		public bool IsImageValid { get; private set; }
		private UnityEngine.Object _advertisementObject = null;
		public UnityEngine.Object AdvertisementObject
		{
			get { return _advertisementObject; }
			set 
			{  
				if(value==null || ( ((Texture2D)value).height != 80 && ((Texture2D)value).width != 200) )
					IsImageValid = false;
				else
				{
					try
					{
						((Texture2D)value).GetPixel(0,0);
						IsImageValid = true;
					}
					catch
					{
						IsImageValid = false;
					}
				}
				
				
				_advertisementObject = value;	
			}
		}
		
		public Texture2D AdvertisementImage
		{
			get
			{
				if(_advertisementObject != null)
					return _advertisementObject as Texture2D;
				
				return null;
			}
		}
		
		public bool IsEmailValid { get; private set; }
		private string _emailAddress = "";
		public string EmailAddress 
		{
			get { return _emailAddress; }
			
			set
			{
				if(string.IsNullOrEmpty(value) || (!value.Contains("@") || !value.Contains(".")))
					IsEmailValid = false;
				else
					IsEmailValid = true;
				
				_emailAddress = value;
			}
		}
		
		public bool IsUrlValid { get; private set; }
		private string _urlLink = "";
		public string UrlLink 
		{
			get { return _urlLink; }
			
			set
			{
				if(string.IsNullOrEmpty(value) || (!value.ToLower().StartsWith("http:") &&  !value.ToLower().StartsWith("https:") ) )
					IsUrlValid = false;
				else
					IsUrlValid = true;
				
				_urlLink = value;
			}
		}
		
		public void ResetSubmission()
		{
			SubmissionId = "";
			SubmissionError = "";
		}
		
		public bool SubmissionHasError { get { return !string.IsNullOrEmpty(SubmissionError); } }
		public string SubmissionError = "";
		public string SubmissionId = "";
	}
}