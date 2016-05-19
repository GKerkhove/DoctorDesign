using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.apis.mantisbt.models
{
	[Serializable]
	public class IssueData
	{
		//public string id;
		
		//public models.ObjectRef view_state;
		
		//public DateTime last_updated;
		
		public models.ObjectRef project;
		
		public string category;
		
		//public models.ObjectRef priority;
		
		public models.ObjectRef severity;
		
		//public models.ObjectRef status;
		
		//public models.AccountData reporter;
		
		public string summary;
		
		//public string version;
		
		//public string build;
		
		//public string platform;
		
		//public string os;
		
		//public string os_build;
		
		//public models.ObjectRef reproducibility;
		
		//public DateTime date_submitted;

		//public string sponsorship_total;
		
		//public AccountData handler;
		
		//public models.ObjectRef projection;
		
		//public models.ObjectRef eta;
		
		//public models.ObjectRef resolution;
		
		//public string fixed_in_version;
		
		public string description;
		
		//public string steps_to_reproduce;
		
		public string additional_information;
		
		//public AttachmentData[] attachments;
		
		//public RelationshipData[] relationships;
		
		//public IssueNoteData[] notes;
		
		//public CustomFieldValueForIssueData[] custom_fields;
	}
}
