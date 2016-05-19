using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable, SoapMessageAttribute("mc_version", "mantis", "http://futureware.biz/mantisconnect")]
	public class VersionRequest : SoapMessage
	{
		[XmlElement("username")]
		public string Username;
		[XmlElement("password")]
		public string Password;
	}
}