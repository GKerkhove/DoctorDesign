using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable, SoapMessageAttribute("mc_projects_get_user_accessibleResponse", "http://futureware.biz/mantisconnect")]
	public class ProjectDataResponse : SoapMessage	
	{
		[XmlElement("return", Namespace = "")]
		public models.ProjectList Projects;
	}
}
