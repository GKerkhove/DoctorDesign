using UnityEngine;

using System;
using System.Collections;
using System.Reflection;

using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;

namespace hg.ApiWebKit.apis
{
	public abstract class ApiBehavior<T> : MonoBehaviour  where T : class 
	{
		[Multiline(8)]
		public string RequestQuickView;
		[Multiline(8)]
		public string ResponseQuickView;
		
		public HttpOperation Operation;
		public HttpRequestModel.HttpRequestModelResult Request;
		public HttpResponse Response;

		public float WaitWhenBusy = 0.1f;
		private bool _isBusy;

		public ApiBehaviorStatus Status = ApiBehaviorStatus.NONE;
		private bool _completed = false;
		public string CompletionTime = DateTime.MinValue.ToString();

		public Action<T,ApiBehaviorStatus> OnCompleteNotification = null;

		public virtual IEnumerator ExecuteAndWait()
		{
			while(_isBusy)
			{
				Configuration.Log("'" + this.GetType().FullName + "' behavior is busy.  Waiting..." ,LogSeverity.WARNING);
				yield return new WaitForSeconds(WaitWhenBusy);
			}

			reset();

			ExecutableCode();
			
			while(!_completed)
				yield return null;

			yield break;
		}

		public virtual void Execute()
		{
			if(_isBusy)
			{
				Configuration.Log("'" + this.GetType().FullName + "' behavior is busy.  Try again later or create another instance." ,LogSeverity.WARNING);
				return;
			}

			reset();

			ExecutableCode();
		}

		public virtual void ExecutableCode()
		{

		}

		private void reset()
		{
			_isBusy = true;
			Status = ApiBehaviorStatus.NONE;
			_completed = false;
			CompletionTime = DateTime.MinValue.ToString();

			foreach(FieldInfo fi in this.GetType().GetFields())
			{
				if(fi.IsDefined(typeof(NullifyOnQueryAttribute),true))
					fi.SetValue(this,null);
			}
		}

		protected void OnCompletion(HttpOperation operation, HttpResponse response, ApiMonitor apiMonitor)
		{
			if(apiMonitor != null)
				apiMonitor.Aggregate(response);

			RequestQuickView = response.Request.RequestModelResult.Summary();
			ResponseQuickView = response.Summary();
			Operation = operation;
			Request = response.Request.RequestModelResult;
			Response = response;
			CompletionTime = DateTime.UtcNow.ToString();
			_completed = true;
			if(OnCompleteNotification != null)
				OnCompleteNotification(this as T, Status);
			_isBusy = false;
		}
	}
}
