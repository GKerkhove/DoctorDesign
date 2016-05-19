using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class StringToBytes : IValueConverter
	{
		public object Convert (object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				byte[] bytes = UTF8Encoding.UTF8.GetBytes((string)input);

				successful = true;

				return bytes;
			}
			catch 
			{
				return null;
			}
		}
	}
}

