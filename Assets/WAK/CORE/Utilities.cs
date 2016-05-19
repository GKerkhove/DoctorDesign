using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit
{
	public static partial class Utilities
	{
		public static string InstanceToString(object instance)
		{
			string fieldInformation = "";
			
			foreach(FieldInfo fi in instance.GetType().GetFields())
			{
				fieldInformation += "<b>[" + fi.Name + "]</b> = " + fi.GetValue(instance) + "\n";
			}
			
			return
				"<color=red>{Class}</color> <color=white><b>[" + instance.GetType().FullName + "]</b></color>\n"+
					"<color=white>{Public Fields}</color> \n"+
					fieldInformation +
					"\n\n";
		}

		public static void Send<T>(
			this T operation, 
			Action<T, HttpResponse> on_success = null, 
			Action<T, HttpResponse> on_failure = null, 
			Action<T, HttpResponse> on_complete = null,
			params string[] parameters) where T : HttpOperation
		{
			operation["on-complete"] = 
				new Action<T, HttpResponse>
					((self, response) => 
					 { 
						if(on_success !=null && (response.Is2XX || response.Is100) && !self.IsFaulted)
							on_success(self, response);
						
						if(on_failure !=null && ((!response.Is2XX && !response.Is100) || self.IsFaulted))
							on_failure (self, response);
						
						if(on_complete !=null)
							on_complete(self, response);
					});
			
			operation.Send(parameters);
		}
	
		public static void Send<T>(
			this T operation,
			Action<T, HttpRequest> on_start = null, 
			Action<T, HttpResponse> on_success = null, 
			Action<T, HttpResponse> on_failure = null, 
			Action<T, HttpResponse> on_complete = null,
			params string[] parameters) where T : HttpOperation
		{
			operation["on-start"] = 
				new Action<T, HttpRequest>
					((self, http_request) => 
					 { 
						if(on_start !=null)
							on_start(self, http_request);
					});
			
			operation.Send(on_success,on_failure,on_complete,parameters);
		}
		
		public static void Send<T>(
			this T operation,
			string batchName,
			Action<T, HttpRequest> on_start = null, 
			Action<T, HttpResponse> on_success = null, 
			Action<T, HttpResponse> on_failure = null, 
			Action<T, HttpResponse> on_complete = null,
			params string[] parameters) where T : HttpOperation
		{
			//TODO do batching	
		
			operation.Send(on_success,on_failure,on_complete,parameters);
		}

		/*
		public static void SendWithCompletion<T>(
			this T request,
		    Action<T, HttpResponse> on_complete) where T : HttpOperation
		{
			Send(request, null, null, on_complete, null);
		}
		
		public static void SendWithSuccess<T>(
			this T request,
			Action<T, HttpResponse> on_success) where T : HttpOperation
		{
			Send(request, on_success, null, null, null);
		}
		
		public static void SendFireForget<T>(
			this T request,
			params string[] parameters) where T : HttpOperation
		{
			Send(request, null, null, null, parameters);
		}
		
		public static void ExecuteFireForget<T>(
			this T request,
			Action<T, HttpResponse> on_success,
			params string[] parameters) where T : HttpOperation
		{
			Send(request, on_success, null, null, parameters);
		}
		*/

		/*
		public static void ExecutePostLogin<T>(
			this T request,
			MonoBehaviour host,
			params string[] parameters) where T : HttpOperation
		{
			host.StartCoroutine(coro_executePostLogin(request, parameters));
		}
		
		private static IEnumerator coro_executePostLogin(
			HttpOperation request, 
			params string[] parameters)
		{
			float start = Time.realtimeSinceStartup;
			
			while(!hg.nosql.h2flow.NosqlProxy.Instance.IsLoggedIn || Time.realtimeSinceStartup - start > 10f)
				yield return null;
			
			float waited = Time.realtimeSinceStartup - start;
			hg.nosql.Configuration.Log("ExecutePostLogin coroutine waited " + waited.ToString("N4") + "s for login completion.", LogSeverity.INFO);
			
			if(hg.nosql.h2flow.NosqlProxy.Instance.IsLoggedIn)
				Execute(request, null, null, null, parameters);
			
			yield return null;
		}
		*/
	
		/*public static object ConvertUsingOperatorAtRuntime(Type X, Type I, Type O, object obj)
		{
			MethodInfo method = typeof(Utilities).GetMethod("ConvertUsingOperator", BindingFlags.Static | BindingFlags.Public);
			MethodInfo generic = method.MakeGenericMethod(X,I,O);
			
			obj = generic.Invoke(null, new object[] { obj });

			return obj;
		}

		public static O ConvertUsingOperator<X, I, O>(I obj)
		{
			var conversionOperator = typeof(X).GetMethods(BindingFlags.Static | BindingFlags.Public)
				.Where(m => m.Name == "op_Explicit" || m.Name == "op_Implicit")
				.Where(m => m.ReturnType == typeof(O))
				.Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(I))
				.FirstOrDefault();
			
			if (conversionOperator != null)
				return (O)conversionOperator.Invoke(null, new object[]{obj});
			
			throw new Exception("No conversion operator found");
		}*/
	}
}