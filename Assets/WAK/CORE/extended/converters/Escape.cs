using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class Escape : IValueConverter
	{
		public object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				string e = WWW.EscapeURL((string)input);
				successful = true;
				return e;
			}
			catch 
			{
				return null;
			}
		}
	}
}

