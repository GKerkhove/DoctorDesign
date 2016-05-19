using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.zebra.models
{
	[Serializable]
	public class Display : ZebraModel
	{
		[hg.LitJson.JsonProperty("display.contrast")]
		public string Contrast;

		[hg.LitJson.JsonProperty("display.backlight")]
		public string Backlight;

		[hg.LitJson.JsonProperty("display.backlight_on_time")]
		public string BacklightOnTime;

		[hg.LitJson.JsonProperty("display.orientation")]
		public string Orientation;

		[hg.LitJson.JsonProperty("display.language")]
		public string Language;
	}
}