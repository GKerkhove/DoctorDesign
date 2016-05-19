using UnityEngine;
using System;

namespace hg.ApiWebKit.apis.zebra.models
{
	[Serializable]
	public class Wlan : ZebraModel
	{
		
		[hg.LitJson.JsonProperty("wlan.available")]
		public string Available;

		[hg.LitJson.JsonProperty("wlan.ip.addr")]
		public string IPAddress;

		[hg.LitJson.JsonProperty("wlan.signal_strength")]
		public string SignalStrength;

		[hg.LitJson.JsonProperty("wlan.bssid")]
		public string BSSID;

		[hg.LitJson.JsonProperty("wlan.essid")]
		public string ESSID;

		[hg.LitJson.JsonProperty("wlan.security")]
		public string Security;

		[hg.LitJson.JsonProperty("wlan.password")]
		public string Password;

		[hg.LitJson.JsonProperty("wlan.wpa.psk")]
		public string WpaPskPassword;
	}
}