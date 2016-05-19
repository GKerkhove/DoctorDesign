using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class Base64Encode : IValueConverter
	{
		public object Convert (object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				string b64 = System.Convert.ToBase64String(
					UTF8Encoding.UTF8.GetBytes((string)input), 
					Base64FormattingOptions.None); 

				successful = true;

				return b64;
			}
			catch 
			{
				return null;
			}
		}
	}
}

