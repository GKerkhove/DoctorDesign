using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class ToLowerCase : ChangeCase
	{
		public override object Convert (object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			return base.Convert(input, targetField, out successful, "lower");
		}
	}
}

