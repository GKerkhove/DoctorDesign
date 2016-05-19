using System;

namespace hg.ApiWebKit.core.attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HttpPathAttribute : Attribute
	{
		public string BaseUriName;
		public string Path;

		/// <summary>
		/// Absolute API URI.
		/// </summary>
		public string Uri
		{
			get;
			private set;	
		}

		/// <summary>
		/// Constructs a valid API URI.
		/// </summary>
		/// <param name="path">Relative path.  The 'default' base URI will be used.</param>
		public HttpPathAttribute(string path)
		{
			BaseUriName = "default";
			Path = validatePath(path);
			Uri = Configuration.GetBaseUri(BaseUriName) + Path;
		}

		/// <summary>
		/// Constructs a valid API URI.
		/// </summary>
		/// <param name="baseUriName">Base URI name as defined in Configuration or null.  A null value treats the path parameter as an absolute URI.</param>
		/// <param name="path">Relative or absolute path.</param>
		public HttpPathAttribute(string baseUriName, string path)
		{
			BaseUriName = baseUriName;
			Path = validatePath(path);
			Uri = Configuration.GetBaseUri(BaseUriName) + Path;
		}
		
		private string validatePath(string path)
		{
			if(!path.StartsWith("/") && BaseUriName != null)
			{
				Configuration.LogInternal(path + " path must begin with '/'", LogSeverity.WARNING);
				return "/" + path;
			}
			
			return path;
		}
	}
}
