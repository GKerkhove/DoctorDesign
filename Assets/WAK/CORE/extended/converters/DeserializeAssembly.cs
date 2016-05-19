using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using hg.LitJson;
using System.Linq;

namespace hg.ApiWebKit.converters
{
	public class DeserializeAssembly : IValueConverter
	{
		public object Convert (object input, FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				Assembly ass = Assembly.Load((byte[])input);

				successful = true;

				return ass;
			}
			catch (Exception ex)
			{
				Configuration.Log("(DeserializeAssembly)(Convert) Failure : " + ex.Message, LogSeverity.ERROR);
				return null;
			}

		}
	}
}

