using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable]
	public class ObjectRef
	{
		public string id;
		
		public string name;
	}
}
