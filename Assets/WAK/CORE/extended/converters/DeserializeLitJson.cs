using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using hg.LitJson;
using System.Linq;

namespace hg.ApiWebKit.converters
{
	public class DeserializeLitJson : IValueConverter
	{
		public object Convert (object input, FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				//Performance Hog
				/*
				MethodInfo toObjectFromString = typeof(JsonMapper).GetMethods(BindingFlags.Static | BindingFlags.Public)
					.Where(m => m.Name == "ToObject")
						.Where (m => m.IsGenericMethod)
						.Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string))
						.FirstOrDefault();

				MethodInfo toObjectFromStringRuntime = toObjectFromString.MakeGenericMethod(targetField.FieldType);

				object result = toObjectFromStringRuntime.Invoke(null, new object[] { (string)input });
				*/

				object result = JsonMapper.ToObject((string)input, targetField.FieldType);

				successful = true;

				return result;

			}
			catch (Exception ex)
			{
				Configuration.Log("(DeserializeLitJson)(Convert) Failure on field '" + targetField.Name + "' : " + ex.Message, LogSeverity.ERROR);
				if(ex.InnerException!=null)
					Configuration.Log("(DeserializeLitJson)(Convert) Failure-Inner : " + ex.InnerException.Message, LogSeverity.ERROR);
				return null;
			}
		}
	}
}

