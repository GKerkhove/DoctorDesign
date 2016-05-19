using UnityEngine;

using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Net.Sockets;
using hg.ApiWebKit.apis.example.media.operations;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.providers;
using hg.ApiWebKit.core.tcp;


namespace hg.ApiWebKit.apis.zebra
{
	public class ChangeZebraSettings : TcpOperation
	{
		public bool LogVerbose = true;
		public bool LogInformation = true;
		public bool LogWarning = true;
		public bool LogError = true;

		protected override void Start ()
		{
			Configuration.SetSetting("log-VERBOSE", LogVerbose);
			Configuration.SetSetting("log-INFO", LogInformation);
			Configuration.SetSetting("log-WARNING", LogWarning);
			Configuration.SetSetting("log-ERROR", LogError);


			base.Start ();
		}

		#region GUI States
		private TransmissionState _state = TransmissionState.READY_TO_READ;

		private enum TransmissionState
		{
			READY_TO_READ,
			READY_TO_WRITE,
			WAITING_ON_READ,
			WAITING_ON_READ_ARBITRARY,
			WAITING_ON_WRITE,
			ERROR,
			COMPLETE,
			COMPLETE_ARBITRARY
		}
		#endregion

		[SerializeField]
		private GUISkin _skin;

		// cube on which to render the zebra logo from printer's HTTP server
		public Renderer Cube;

		private Type _modelType;
		private models.ZebraModel _requestModel;
		private models.ZebraModel _responseModel;

		private ArbitraryModel _arbitraryModel = new ArbitraryModel();

		private class ArbitraryModel
		{
			public string Request = "";
			public string Response = "";
		}

		#region Tcp Socket callbacks
		// Failure received from Tcp Provider
		protected override void OnFailure (Exception exception)
		{
			_state = TransmissionState.ERROR;
		}


		// Invoked from base after user invokes the Send method.
		//  The return value will be used as the data to send over Tcp Socket
		protected override string OnRequest ()
		{
			if(!string.IsNullOrEmpty(_arbitraryModel.Request))
			{
				return "{}{\"" + _arbitraryModel.Request.Trim() + "\":null}";
			}
			else
			{
				string json = "{}" + hg.LitJson.JsonMapper.ToJson(_requestModel);
				return json;
			}
		}

		// Invoked from base on a successful response
		//  The message parameter is the data received from Tcp Socket in full
		protected override void OnResponse (string message)
		{
			if(!string.IsNullOrEmpty(_arbitraryModel.Request))
			{
				_arbitraryModel.Response = message;
			}
			else
			{
				// deserialize json to object of known model type
				FieldInfo responseField = this.GetType().GetField("_responseModel",BindingFlags.NonPublic|BindingFlags.Instance);
				object model = hg.LitJson.JsonMapper.ToObject(message, _modelType);
				responseField.SetValue(this, model);
			}

			//Debug.Log("#operation success :  " + message);
			
			if(_state == TransmissionState.WAITING_ON_READ)
				_state = TransmissionState.READY_TO_WRITE;
			else if(_state == TransmissionState.WAITING_ON_READ_ARBITRARY)
				_state = TransmissionState.COMPLETE_ARBITRARY;
			else 
				_state = TransmissionState.COMPLETE;
		}
		#endregion

		// Make a request with a typed model
		private void request(Type modelType)
		{
			// cache the model we are using to help in response deserialization
			_modelType = modelType;

			// activate request model to appropriate type
			FieldInfo requestField = this.GetType().GetField("_requestModel",BindingFlags.NonPublic|BindingFlags.Instance);
			object model = Activator.CreateInstance(modelType);
			requestField.SetValue(this, model);

			// activate request model to appropriate type
			FieldInfo responseField = this.GetType().GetField("_responseModel",BindingFlags.NonPublic|BindingFlags.Instance);
			model = Activator.CreateInstance(modelType);
			responseField.SetValue(this, model);

			requestZebraImage();
			
			Send(null,null);

			_state = TransmissionState.WAITING_ON_READ;
		}
		
		#region Get logo.png from HTTP server on printer
		private void requestZebraImage()
		{
			new GetImage() {
				ImageUri = "http://" + OperationSettings.Hostname + "/logo.png"
			}
			.Send(onZebraImageSuccess,null,null);
		}
		
		private void onZebraImageSuccess(GetImage operation, HttpResponse response)
		{
			Cube.material.mainTexture = operation.ImageTexture;
		}
		#endregion

		#region GUI
		void OnGUI()
		{
			if(GUI.skin != _skin)
				GUI.skin = _skin;

			switch(_state)
			{
			case TransmissionState.READY_TO_READ:
				GUI.Label(new Rect(10,50,250,50), "HOSTNAME:");
				OperationSettings.Hostname = GUI.TextField(new Rect(290,50,300,50), OperationSettings.Hostname);
				
				GUI.Label(new Rect(10,120,250,50), "PORT:");
				OperationSettings.Port = int.Parse(GUI.TextField(new Rect(290,120,300,50), OperationSettings.Port.ToString()));
				
				if(GUI.Button(new Rect(10,300,Screen.width - 20,60),"Display"))
				{
					request(typeof(models.Display));
				}

				if(GUI.Button(new Rect(10,365,Screen.width - 20,60),"WLAN"))
				{
					request(typeof(models.Wlan));
				}

				GUI.Label(new Rect(10,440,250,50), "Arbitrary:");
				_arbitraryModel.Request = GUI.TextField(new Rect(290,440,300,50), _arbitraryModel.Request);

				if(_arbitraryModel.Request.Trim().Length > 0)
				{
					if(GUI.Button(new Rect(10,500,Screen.width - 20,60),"Request"))
					{
						_state = TransmissionState.WAITING_ON_READ_ARBITRARY;
						
						requestZebraImage();
					
						Send(null,null);
					}
				}

				break;
				
			case TransmissionState.WAITING_ON_READ:
			case TransmissionState.WAITING_ON_READ_ARBITRARY:
			case TransmissionState.WAITING_ON_WRITE:
				GUI.Label(new Rect(10,50,250,50), "WAIT...");
				
				break;
				
			case TransmissionState.ERROR:
				GUI.Label(new Rect(10,80,250,50), "ERROR!!!");

				if(GUI.Button(new Rect(10,150,Screen.width - 20,200),"Try Again"))
				{
					_arbitraryModel = new ArbitraryModel();
					_state = TransmissionState.READY_TO_READ;
				}

				break;
				
			case TransmissionState.READY_TO_WRITE:

				float nextY = 50;
				
				foreach(FieldInfo field in _responseModel.GetType().GetFields())
				{
					GUI.Label(new Rect(10,nextY,250,50), field.Name);
					object obj = field.GetValue(_responseModel);
					string value = obj==null ? "" : obj.ToString();
					setFieldFromInput(_responseModel, field, GUI.TextField(new Rect(290,nextY,250,50), value));

					nextY += 70;
				}

				if(GUI.Button(new Rect(10,nextY,Screen.width - 20,200),"Write Settings"))
				{
					_state = TransmissionState.WAITING_ON_WRITE;
					_requestModel = _responseModel;
					_responseModel = null;

					Send(null,null);
				}

				break;

			case TransmissionState.COMPLETE:

				float nextY2 = 50;
				
				foreach(FieldInfo field in _responseModel.GetType().GetFields())
				{
					object obj = field.GetValue(_responseModel);
					string value = obj==null ? "" : obj.ToString();
					GUI.Label(new Rect(10,nextY2,Screen.width - 20,50), (string)(field.Name + " : " + value));
					
					nextY2 += 70;
				}

				if(GUI.Button(new Rect(10,nextY2,Screen.width - 20,200),"Try Again"))
				{
					_state = TransmissionState.READY_TO_READ;
				}

				break;

			case TransmissionState.COMPLETE_ARBITRARY:

				GUI.TextArea(new Rect(10,10,Screen.width - 20, Screen.height - 100), _arbitraryModel.Response);

				if(GUI.Button(new Rect(10,Screen.height - 80,Screen.width - 20,60),"Try Again"))
				{
					_arbitraryModel = new ArbitraryModel();
					_state = TransmissionState.READY_TO_READ;
				}
				
				break;
			}
			
		}

		private void setFieldFromInput(object obj, FieldInfo field, string value)
		{
			field.SetValue(obj, value);
		}
		#endregion
	}
}