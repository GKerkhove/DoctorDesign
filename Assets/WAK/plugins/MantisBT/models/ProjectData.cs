using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable]
	public class ProjectData
	{
		public int id;

		public string name;

		public models.ObjectRef status;
		
		public bool enabled;
		
		public models.ObjectRef view_state;
		
		public models.ObjectRef access_min;
		
		public string file_path;
		
		public string description;
	}
}
