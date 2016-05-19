using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace hg.ApiWebKit.providers
{
	public class TcpOneShotClient : MonoBehaviour
	{
		public event EventHandler OnConnectionSuccess;
		public event EventHandler OnConnectionError;
		public event EventHandler OnMessage;
		public event EventHandler OnFailedTransmission;

		private TcpClient _client;

		private byte[] _readBuffer;
		private int _readBufferSize = 512;

		private string _dataIn = "";
		private string _dataOut = "";

		private string _targetHost;
		private int _targetPort;

		private bool _inUse = false;

		private float _connectTimeout = 9f;
		private float _receiveTimeout = 9f;
		private float _sendTimeout = 9f;

		private Exception _commsException = null;
		private Thread _commsThread = null;

		private CommsStatus _commsStatus = CommsStatus.NONE;

		private enum CommsStatus
		{
			NONE,
			CONNECT_SUCCESS,
			CONNECT_FAIL,
			COMMS_FAIL,
			MESSAGE_RECEIVED,
			RESET_REQUESTED
		}

		private void Update()
		{
			switch(_commsStatus)
			{
			case CommsStatus.COMMS_FAIL:

				_commsStatus = CommsStatus.RESET_REQUESTED;

				if(OnFailedTransmission!=null)
					OnFailedTransmission(_commsException,null);

				break;

			case CommsStatus.CONNECT_FAIL:

				_commsStatus = CommsStatus.RESET_REQUESTED;

				if(OnConnectionError!=null)
					OnConnectionError(_commsException,null);

				break;

			case CommsStatus.CONNECT_SUCCESS:

				_commsStatus = CommsStatus.NONE;

				if(OnConnectionSuccess!=null)
					OnConnectionSuccess(this,null);

				break;

			case CommsStatus.MESSAGE_RECEIVED:

				_commsStatus = CommsStatus.RESET_REQUESTED;

				if(OnMessage!=null)
					OnMessage(_dataIn,null);

				break;

			case CommsStatus.RESET_REQUESTED:

				_commsStatus = CommsStatus.NONE;

				reset(_commsException);

				break;

			default:

				break;
			}
		}

		public void Setup(string host, int port)
		{
			Setup(host,port,-1,_connectTimeout,_receiveTimeout,_sendTimeout);
		}

		public void Setup(string host, int port, int bufferSize, float connectTimeout, float receiveTimeout, float sendTimeout)
		{
			_targetHost = host;
			_targetPort = port;
			_readBufferSize = bufferSize;
			_connectTimeout = connectTimeout;
			_receiveTimeout = receiveTimeout;
			_sendTimeout = sendTimeout;
		}

	
		public bool Send(string data)
		{
			Configuration.Log("TCP OneShot : Send Requested.",LogSeverity.VERBOSE);

			if(_inUse)
			{
				Configuration.Log("TCP OneShot : Send Request Denied.  Socket in-use.",LogSeverity.WARNING);
				return false;
			}

			_inUse = true;

			_client = new TcpClient();

			if(_readBufferSize < 1)
				_readBufferSize = _client.ReceiveBufferSize;

			_readBuffer  =  new byte[_readBufferSize];

			_dataOut = data;

			ThreadStart ts = new ThreadStart(connect);
			_commsThread = new Thread(ts);
			_commsThread.Priority = System.Threading.ThreadPriority.Normal;
			_commsThread.Name = "TcpOneShotClient2_CommsThread";
			_commsThread.Start();

			return true;
		}

		private void reset(Exception ex)
		{
			Configuration.Log("TCP OneShot : Resetting.",LogSeverity.VERBOSE);

			try { _client.Close(); } catch {}

			_commsThread.Join(1);
			Configuration.Log("TCP OneShot : Comms Thread Joined.",LogSeverity.VERBOSE);

			_dataOut = "";
			_dataIn = "";

			_commsException = null;
			_commsThread = null;

			_client = null;

			_inUse = false;
		}

		private void connect()
		{
			try
			{
				Configuration.Log("TCP OneShot : BeginConnect",LogSeverity.VERBOSE);
				IAsyncResult result = _client.BeginConnect(_targetHost, _targetPort, null, null);

				Configuration.Log("TCP OneShot : BeginConnect Timeout Delay = " + _connectTimeout,LogSeverity.VERBOSE);
				result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(_connectTimeout));
				if (!_client.Connected)
				{
					Configuration.Log("Connect Timed Out.",LogSeverity.WARNING);
					throw new TimeoutException();
				}

				_client.EndConnect(result);
				Configuration.Log("TCP OneShot : EndConnect",LogSeverity.VERBOSE);
				
				_commsStatus = CommsStatus.CONNECT_SUCCESS;
				
				write();
			}
			catch (Exception ex)
			{
				Configuration.Log("Connect Error: " + ex.Message,LogSeverity.ERROR);

				_commsException = ex;
				_commsStatus = CommsStatus.CONNECT_FAIL;
			}
		}

		private void read()
		{
			try
			{
				Configuration.Log("TCP OneShot : BeginRead",LogSeverity.VERBOSE);

				IAsyncResult result = _client.GetStream()
					.BeginRead(_readBuffer, 0, _readBufferSize, null, null);

				Configuration.Log("TCP OneShot : BeginRead Timeout Delay = " + _receiveTimeout,LogSeverity.VERBOSE);
				bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(_receiveTimeout));
				if (!success)
				{
					Configuration.Log("Read Timed Out.",LogSeverity.WARNING);
					throw new TimeoutException();
				}

				int bytesRead = _client.GetStream().EndRead(result);
				Configuration.Log("TCP OneShot : EndRead",LogSeverity.VERBOSE);
				
				//Debug.LogWarning("Read : " + bytesRead);
				
				_dataIn += Encoding.ASCII.GetString(_readBuffer, 0, bytesRead);
				
				if(_client.Available < 1)
				{
					Configuration.Log("Message Received!",LogSeverity.VERBOSE);

					_commsStatus = CommsStatus.MESSAGE_RECEIVED;

					return;
				}
				
				read();
			}
			catch (Exception ex)
			{
				Configuration.Log("Read Error: " + ex.Message,LogSeverity.ERROR);

				_commsException = ex;
				_commsStatus = CommsStatus.COMMS_FAIL;
			}
		}

		private void write()
		{
			try
			{
				Configuration.Log("TCP OneShot : BeginWrite",LogSeverity.VERBOSE);
				Configuration.Log("TCP OneShot : Write Buffer = " + _dataOut,LogSeverity.VERBOSE);
				
				byte[] buffer = Encoding.ASCII.GetBytes(_dataOut);
				
				IAsyncResult result = _client.GetStream()
					.BeginWrite(buffer, 0, buffer.Length, null, null);

				Configuration.Log("TCP OneShot : BeginWrite Timeout Delay = " + _sendTimeout,LogSeverity.VERBOSE);
				bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(_sendTimeout));
				if (!success)
				{
					Configuration.Log("Read Timed Out.",LogSeverity.WARNING);
					throw new TimeoutException();
				}

				_client.GetStream().EndWrite(result);
				Configuration.Log("TCP OneShot : EndWrite",LogSeverity.VERBOSE);
				
				read();
			}
			catch (Exception ex)
			{
				Configuration.Log("BeginWrite Error: " + ex.Message,LogSeverity.ERROR);
				
				_commsException = ex;
				_commsStatus = CommsStatus.COMMS_FAIL;
			}
		}
	}
}