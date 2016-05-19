using UnityEngine;
using System.Collections;

namespace hg.ApiWebKit.apis.google.cloudStorage.models
{
	public class BucketResources
	{
		public string kind;
		public BucketResource[] items;
	}

	public class BucketResource
	{
		public string kind;
		public string id;
		public string selfLink;
		public string projectNumber;
		public string name;
		public string timeCreated;
		public string metageneration;
		public string location;
		public string storageClass;
		public string etag;
		public ObjectOwner owner;
		public ObjectAccessControlResource[] acl;
		public ObjectAccessControlResource[] defaultObjectAcl;
		
	}
}