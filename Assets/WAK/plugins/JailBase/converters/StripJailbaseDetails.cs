using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace hg.ApiWebKit.apis.jailbase.converters
{
	[Obsolete]
	public class StripJailbaseDetails : IValueConverter
	{
		public object Convert (object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				string json = (string)input;
				json = Regex.Replace(json,"\"details\":(.*?)]],","");

				successful = true;

				return json;
			}
			catch 
			{
				return null;
			}
		}
	}
}

