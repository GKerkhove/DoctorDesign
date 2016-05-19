using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Person : ModelBase
	{
		public string _key;
		public string id;
		public string firstName;
		public string lastName;
		public string maidenName;
		public string formattedName;
		public string phoneticFirstName;
		public string phoneticLastName;
		public string formattedPhoneticName;
		public string headline;
		public Location location;
		public string industry;
		public int distance;
		public Share currentShare;
		public int numConnections;
		public bool numConnectionsCapped;
		public string summary;
		public string specialties;
		public Positions positions;
		public string pictureUrl;
		public StandardProfile siteStandardProfileRequest;
		public ApiProfile apiStandardProfileRequest;
		public string publicProfileUrl;
		public string emailAddress;
		public Connections connections; //TODO: fix serialization depth issue with 4.5
		public PhoneNumbers phoneNumbers;
	}
}

