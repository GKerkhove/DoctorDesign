using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable, SoapMessageAttribute("mc_versionResponse", "http://futureware.biz/mantisconnect")]
	public class VersionResponse : SoapMessage
	{
		[XmlElement("return", Namespace= "")]
		public string Version;
	}
}