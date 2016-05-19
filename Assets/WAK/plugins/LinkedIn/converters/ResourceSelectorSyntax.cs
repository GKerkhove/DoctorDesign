using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.apis.linkedin.converters
{
	public class ResourceSelectorSyntax : IValueConverter
	{
		public virtual object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			StringBuilder sb = new StringBuilder();
			sb.Append("::(");
			
			if(input == null)
				sb.Append("~");
			else
			{
				//string[] resourceIdentifiers = ((string)input).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries); 

				System.Collections.Generic.List<string> rids = new System.Collections.Generic.List<string>();
				
				foreach(string resourceIdentifier in (string[])input)
				{
					if(resourceIdentifier == "~" || resourceIdentifier.ToLower() == "self" || resourceIdentifier.ToLower() == "mine")
					{
						rids.Add("~");
					}
					else if(resourceIdentifier.ToLower().StartsWith("id="))
					{
						rids.Add(resourceIdentifier);
					}
					else if(resourceIdentifier.ToLower().StartsWith("url="))
					{
						string url = resourceIdentifier.Substring(resourceIdentifier.IndexOf("="),resourceIdentifier.Length - resourceIdentifier.IndexOf("="));
						rids.Add("url="+WWW.EscapeURL(url));
					}
					else if(resourceIdentifier.ToLower().StartsWith("http:") || resourceIdentifier.ToLower().StartsWith("https:"))
					{
						rids.Add("url="+WWW.EscapeURL(resourceIdentifier));
					}
					else
					{
						rids.Add("id="+resourceIdentifier);
					}
				}
				
				sb.Append(string.Join(",",rids.ToArray()));
			}
			
			sb.Append(")");

			successful = true;
			return sb.ToString();
		}
	}
}

