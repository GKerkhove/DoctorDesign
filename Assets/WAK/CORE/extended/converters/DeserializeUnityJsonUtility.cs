#if UNITY_5_3
using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Linq;

namespace hg.ApiWebKit.converters
{
	public class DeserializeUnityJsonUtility : IValueConverter
	{
		public object Convert (object input, FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				object result = JsonUtility.FromJson((string)input, targetField.FieldType);

				successful = true;

				return result;

			}
			catch (Exception ex)
			{
				Configuration.Log("(DeserializeUnityJsonUtility)(Convert) Failure on field '" + targetField.Name + "' : " + ex.Message, LogSeverity.ERROR);
				if(ex.InnerException!=null)
					Configuration.Log("(DeserializeUnityJsonUtility)(Convert) Failure-Inner : " + ex.InnerException.Message, LogSeverity.ERROR);
				return null;
			}
		}
	}
}
#endif