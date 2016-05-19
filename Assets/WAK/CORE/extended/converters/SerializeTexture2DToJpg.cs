using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class SerializeTexture2DToJpg : IValueConverter
	{
		public object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				byte[] b = ((Texture2D)input).EncodeToJPG();
				successful = true;
				return b;
			}
			catch 
			{
				return null;
			}
		}
	}
}

