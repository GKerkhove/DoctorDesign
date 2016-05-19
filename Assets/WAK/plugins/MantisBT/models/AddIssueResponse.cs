using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable, SoapMessageAttribute("mc_issue_addResponse", "http://futureware.biz/mantisconnect")]
	public class AddIssueResponse : SoapMessage	
	{
		[XmlElement("return", Namespace = "")]
		public string IssueId;
	}
}
