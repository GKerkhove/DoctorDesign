using UnityEngine;
using System.Collections;

namespace hg.ApiWebKit.apis.google.cloudStorage.models
{
	public class ObjectResources
	{
		public string kind;
		public ObjectResource[] items;
	}

	public class ObjectResource
	{
		public string kind;
		public string id;
		public string selfLink;
		public string name;
		public string bucket;
		public string generation;
		public string metageneration;
		public string contentType;
		public string updated;
		public string timeDeleted;
		public string storageClass;
		public string size;
		public string md5Hash;
		public string mediaLink;
		public string contentEncoding;
		public string contentDisposition;
		public string contentLanguage;
		public string cacheControl;
		public string crc32c;
		public string componentCount;
		public string etag;
		public ObjectOwner owner;
		public ObjectAccessControlResource[] acl;
		
	}
	
	public class ObjectOwner
	{
		public string entity;
		public string entityId;
	}
	
	public class ObjectAccessControlResource
	{
		public string kind;
		public string id;
		public string selfLink;
		public string bucket;
		public string @object;
		public string generation;
		public string entity;
		public string role;
		public string email;
		public string entityId;
		public string domain;
		public string etag;
		public ProjectTeam projectTeam;
	}
	
	public class ProjectTeam
	{
		public string projectNumber;
		public string team;
	}
}