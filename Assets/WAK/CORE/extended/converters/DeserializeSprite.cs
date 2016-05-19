using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class DeserializeSprite : DeserializeTexture2D
	{
		public override object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				Texture2D texture = (Texture2D)base.Convert(input, targetField, out successful, parameters);

				Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.zero);

				successful = true;
				return sprite;
			}
			catch
			{
				return null;
			}
		}
	}
}

