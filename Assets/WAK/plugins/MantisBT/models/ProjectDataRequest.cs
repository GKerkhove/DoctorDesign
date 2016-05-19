using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable, SoapMessageAttribute("mc_projects_get_user_accessible", "mantis", "http://futureware.biz/mantisconnect")]
	public class ProjectDataRequest : SoapMessage
	{
		[XmlElement("username")]
		public string Username;
		[XmlElement("password")]
		public string Password;
	}
}
