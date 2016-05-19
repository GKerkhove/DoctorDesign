using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public abstract class ChangeCase : IValueConverter
	{
		public virtual object Convert (object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			input = System.Convert.ChangeType(input,typeof(string));

			if(parameters==null || parameters[0].ToString().ToLower() == "lower")
			{
				try
				{
					string l = ((string)input).ToLower();
					successful = true;
					return l;
				}
				catch
				{
					return null;
				}
			}
			else if(parameters[0].ToString().ToLower() == "upper")
			{
				try
				{
					string u =  ((string)input).ToUpper();
					successful = true;
					return u;
				}
				catch
				{
					return null;
				}
			}
			else
				return input;
		}
	}
}

