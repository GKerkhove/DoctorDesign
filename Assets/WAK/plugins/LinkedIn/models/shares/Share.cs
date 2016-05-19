using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.linkedin.models
{
	[Serializable]
	public class Share : ModelBase
	{
		public Attribution attribution;

		public string id ;
		public long timestamp ;
		public string description;
		public string comment ;
		public Author author ;
		public Content content ;
		public Source source ;
		public Visibility visibility ;
	}

	[Serializable]
	public class Attribution : ModelBase
	{
		public Share share;
	}

	[Serializable]
	public class Author : ModelBase
	{
		public string firstName ;
		public string id ;
		public string lastName ;
	}

	[Serializable]
	public class Content : ModelBase
	{
		public string description ;
		public string eyebrowUrl ;
		public string resolvedUrl ;
		public string shortenedUrl ;
		public string submittedUrl ;
		public string submittedImageUrl ;
		public string thumbnailUrl;
		public string title ;
	}

	[Serializable]
	public class ServiceProvider : ModelBase
	{
		public string name ;
	}

	[Serializable]
	public class SharingApplication : ModelBase
	{
		public string name ;
	}

	[Serializable]
	public class Source : ModelBase
	{
		public ServiceProvider serviceProvider ;
		public SharingApplication application;
		public string serviceProviderAccountHandle;
		public string serviceProviderAccountId;
		public string serviceProviderShareId;
	}

	[Serializable]
	public class Visibility : ModelBase
	{
		public string code ;
	}
}

