using UnityEngine;
using System;
using System.Text;

namespace hg.ApiWebKit.converters
{
	public class DeserializeTexture2D : IValueConverter
	{
		public virtual object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

#if UNITY_EDITOR && !UNITY_EDITOR_OSX && !UNITY_IPHONE
//Attempt GIF conversion (Windows only)

			try
			{
				Texture2D texture = new Texture2D(1,1);
				
				byte[] magicBytes = new byte[6];
				Buffer.BlockCopy((byte[])input, 0, magicBytes, 0, 6);
				string magicText = System.Text.Encoding.UTF8.GetString(magicBytes);
				if(magicText.ToLower().Contains("gif"))
				{
					var ms = new System.IO.MemoryStream((byte[])input);
					var image = System.Drawing.Image.FromStream(ms);
					var dimension = new System.Drawing.Imaging.FrameDimension(image.FrameDimensionsList[0]);
					image.SelectActiveFrame(dimension,0);
					var frame = new System.Drawing.Bitmap(image.Width,image.Height);
					System.Drawing.Graphics.FromImage(frame).DrawImage(image, System.Drawing.Point.Empty);
					texture = new Texture2D(frame.Width,frame.Height);

					for(int x=0; x<frame.Width; x++)
					{
						for(int y=0; y<frame.Height; y++)
						{
							var sourceColor = frame.GetPixel(x,y);
							texture.SetPixel(frame.Width - 1 - x, y, new Color32(sourceColor.R,sourceColor.G,sourceColor.B,sourceColor.A));
						}
					}

					texture.Apply();

					//TODO OSX IMPL
					//http://answers.unity3d.com/questions/53170/using-drawing-package-like-systemdrawing.html
				}
				else
				{
					texture.LoadImage((byte[])input);
				}
				
				successful = true;
				return texture;
			}
			catch(Exception ex)
			{
				Configuration.Log("[DeserializeTexture2D+GIF] " + ex.Message);
				return null;
			}
			
#else
//PNG and JPG only

			try
			{
				Texture2D texture = new Texture2D(1,1, TextureFormat.ARGB32, false);
				texture.LoadImage((byte[])input);
				
				successful = true;
				return texture;
			}
			catch 
			{
				return null;
			}
#endif
		}
	}
}

