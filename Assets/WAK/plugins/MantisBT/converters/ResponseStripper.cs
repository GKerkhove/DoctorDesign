using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace hg.ApiWebKit.apis.mantisbt.converters
{
	public class ResponseStripper : IValueConverter
	{
		public object Convert (object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				string xml = (string)input;
				xml = Regex.Replace(xml,"<return(.*?)>","<return>");
				xml = Regex.Replace(xml,"xsi:type=\"ns1(.*?)\"", "");

				successful = true;

				return xml;
			}
			catch 
			{
				return null;
			}
		}
	}
}

