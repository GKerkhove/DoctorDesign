using UnityEngine;
using System;
using hg.ApiWebKit.providers;

namespace hg.ApiWebKit.core.tcp
{
	public class TcpOperation : MonoBehaviour
	{
		// OneShot is the only available Tcp provider at this time
		[HideInInspector]
		private TcpOneShotClient _client;

		// Tcp destination and operating parameters
		public TcpPath OperationSettings;

		protected virtual void Start()
		{

		}

		protected virtual void OnFailure(Exception exception)
		{

		}

		protected virtual string OnRequest()
		{
			return null;
		}

		protected virtual void OnResponse(string message)
		{

		}

		public void Send(Action<string> onMessage, Action<Exception> onFailure)
		{
			// initialize provider and add to scene
			_client = (TcpOneShotClient)Configuration.Bootstrap().AddComponent(OperationSettings.ProviderType);
			_client.Setup(OperationSettings.Hostname, OperationSettings.Port, OperationSettings.ReadBufferSize, OperationSettings.ConnectTimeout, OperationSettings.ReceiveTimeout, OperationSettings.SendTimeout);

			_client.OnMessage += (object sender, EventArgs e) => {
				if(onMessage != null)
					onMessage((string)sender);

				OnResponse((string)sender);

				operationCompleted();
			};

			_client.OnFailedTransmission += (object sender, EventArgs e) => {
				if(onFailure != null)
					onFailure((Exception)sender);

				OnFailure((Exception)sender);

				operationCompleted();
			};

			_client.OnConnectionError += (object sender, EventArgs e) => {
				if(onFailure != null)
					onFailure((Exception)sender);

				OnFailure((Exception)sender);

				operationCompleted();
			};

			_client.OnConnectionSuccess += (object sender, EventArgs e) =>  {

			};

			// get the data to send from derived class
			string data = OnRequest();

			_client.Send(data);
		}

		protected virtual void Update()
		{
			if(_destroyProvider)
			{
				_destroyProvider = false;
				GameObject.Destroy(_client);
			}
		}

		private bool _destroyProvider = false;

		void operationCompleted()
		{
			_destroyProvider = true;
		}
	}
}