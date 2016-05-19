using UnityEngine;
using System;

namespace hg.ApiWebKit.core.tcp
{
	[Serializable]
	public class TcpPath
	{
		public string Hostname;
		public int Port;
		public int ReadBufferSize = 512;
		public float ConnectTimeout = 10f;
		public float ReceiveTimeout = 10f;
		public float SendTimeout = 10f;

		[HideInInspector]
		public Type ProviderType = typeof(hg.ApiWebKit.providers.TcpOneShotClient);
	}
}