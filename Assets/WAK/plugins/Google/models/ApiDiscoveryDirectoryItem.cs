using UnityEngine;
using System.Collections;

namespace hg.ApiWebKit.apis.google.models
{
	public class ApiDiscoveryDirectoryItem
	{
		public string kind;
		public string id;
		public string name;
		public string version;
		public string title;
		public string description;
		public string discoveryRestUrl;
		public string discoveryLink;
		public string documentationLink;
		public bool preferred;
		public string[] labels;
		public ApiResourceIcons icons;
		
		public Texture2D Image;
	}
}