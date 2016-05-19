using UnityEngine;
using System;

namespace hg.ApiWebKit
{
	public interface IValueConverter
	{
		object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters);
	}
}