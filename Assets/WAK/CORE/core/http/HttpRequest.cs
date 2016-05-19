using UnityEngine;
using System;

namespace hg.ApiWebKit.core.http
{
	public class HttpRequest
	{
		public bool 					WasCancelled	{ get { return CompletionState == HttpRequestState.CANCELLED; } }
		public HttpRequestState 		CompletionState { get; internal set; }
		public HttpAbstractProvider 	Client 			{ get; internal set; }
		public HttpRequestModel.HttpRequestModelResult RequestModelResult { get; internal set; }

		public HttpRequest(HttpRequestModel.HttpRequestModelResult requestModelResult)
		{
			RequestModelResult = requestModelResult;
		}

		public void Cancel()
		{
			if(Client != null) Client.Reset();
		}

		public static bool Send(HttpRequest request,
		                        System.Action<HttpResponse> onCompleteCallback,
		                        System.Action<float, float, float> onTransferProgressUpdateCallback,
		                        System.Action<HttpRequestState, HttpRequestState> onStateChangeCallback)
		{
			Action<HttpRequest> httpStartCallback = Configuration.GetSetting<Action<HttpRequest>>("on-http-start");
			if(httpStartCallback != null) httpStartCallback(request);
			
			request.RequestModelResult.Operation.InvokeUserAction("on-start", request.RequestModelResult.Operation, request);
			
			HttpAbstractProvider client = (HttpAbstractProvider)Configuration.Bootstrap().AddComponent(request.RequestModelResult.ProviderType);

			return client.Send(
				request,
				onCompleteCallback,
				onTransferProgressUpdateCallback,
				onStateChangeCallback
			);
		}
		

	}
}