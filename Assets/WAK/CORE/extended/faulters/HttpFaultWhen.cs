using System;
using hg.ApiWebKit.core.http;
using hg.ApiWebKit.core.attributes;
using System.Reflection;

namespace hg.ApiWebKit.faulters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class HttpFaultWhenAttribute : HttpFaultAttribute
	{
		public string FieldName = null;
		public HttpFaultWhenCondition FieldOperator = HttpFaultWhenCondition.Unset;
		public object FieldValue = null;

		public HttpFaultWhenAttribute(string fieldName, HttpFaultWhenCondition fieldOperator, object fieldValue): base()
		{
			FieldName = fieldName;
			FieldOperator = fieldOperator;
			FieldValue = fieldValue;
		}

		public override void CheckFaults (HttpOperation operation, HttpResponse response)
		{
			string[] dotNotation = FieldName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
			
			FieldInfo info = null;
			object context = operation;
			
			foreach(string dotNotated in dotNotation)
			{
				if(context==null)
					break;
			
				info = context.GetType().GetField(dotNotated, BindingFlags.Public | BindingFlags.Instance);
				
				if(info==null)
					throw new NotSupportedException("HttpFaultWhen could not find '" + dotNotated + "' field when parsing '" + FieldName + "'");
				
				context = info.GetValue(context);
			}
		
			switch(FieldOperator)
			{
				case HttpFaultWhenCondition.Is:
					if(object.Equals(context,FieldValue))	//UNITY 501f1: operator == stopped working
						operation.Fault("Field '" + FieldName + "' is " + (FieldValue==null ? "(null)" : FieldValue.ToString()) + " on response.");
					
					break;
				
				case HttpFaultWhenCondition.IsNot:
					if(!object.Equals(context,FieldValue))	//UNITY 501f1: operator == stopped working
						operation.Fault("Field '" + FieldName + "' is not " + (FieldValue==null ? "(null)" : FieldValue.ToString()) + " on response.");
				
					break;
			}
		}
	}
}
