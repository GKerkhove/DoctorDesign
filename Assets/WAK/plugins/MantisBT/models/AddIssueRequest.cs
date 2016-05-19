using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable, SoapMessageAttribute("mc_issue_add", "mantis", "http://futureware.biz/mantisconnect")]
	public class AddIssueRequest : SoapMessage
	{
		[XmlElement("username")]
		public string Username;
		[XmlElement("password")]
		public string Password;
		[XmlElement("issue")]
		public models.IssueData IssueData;
	}
}