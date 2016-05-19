using System;
using System.Reflection;
using hg.ApiWebKit;
using System.Linq;
using hg.ApiWebKit.core.http;

namespace hg.ApiWebKit.core.attributes
{
	public abstract class HttpMappedValueAttribute : Attribute
	{
		public string VariableName;
		public string VariableValue;
		public string Value;

		public Type Converter;
		public Type[] Converters;

		public bool MapOnRequest()
		{
			return Direction == MappingDirection.REQUEST || Direction == MappingDirection.ALL;
		}

		public bool MapOnResponse()
		{
			return Direction == MappingDirection.RESPONSE || Direction == MappingDirection.ALL;
		}

		public MappingDirection Direction 
		{
			get;
			private set;
		}

		public string Name 
		{
			get;
			private set;
		}

		protected HttpMappedValueAttribute(MappingDirection direction, string name)
		{
			Direction = direction;
			Name = name;
		}

		public void Initialize()
		{
			//BUG: IL2CPP - causes the array to be of wrong size 
			if(Converter != null)
			{
				if(Converters == null)
				{
					Converters = new Type[] { Converter };
				}
				else
				{
					Array.Resize(ref Converters, Converters.Length + 1);
					Converters[Converters.Length - 1] = Converter;
				}
			}
		}

		/* response */


		// GET THE ARTIFACT NAME
		//  resolve the name 
		//  name could be:
		//  - header name
		//  it can come from the attribute, fieldInfo.name or a config variable
		// [SomeAttribute(Name = "staticHeaderName")]
		// [SomeAttribute()]
		// [SomeAttribute(VariableName = "config.some.name")]
		public virtual string OnResponseResolveName(HttpOperation operation, FieldInfo fi, HttpResponse response)
		{
			if(VariableName != null && Configuration.HasSetting(VariableName))
			{
				return Configuration.GetSetting<string>(VariableName);
			}
			else if(!string.IsNullOrEmpty(this.Name))
			{
				return this.Name;
			}
			else
			{
				return fi.Name;
			}
		}

		// GET THE RAW VALUE
		//  value could come from the attribute, fieldInfo.getValue(), from a config variable
		//   but most likely the value will either come from response headers or response text/data
		// [SomeAttribute(Value = "staticValue")]
		// [SomeAttribute(Value = "some,array,of,values")]
		// [SomeAttribute()]
		// [SomeAttribute(VariableValue = "config.some.value")]
		public virtual object OnResponseResolveValue(string name, HttpOperation operation, FieldInfo fi, HttpResponse response)
		{
			if(VariableValue != null && Configuration.HasSetting(VariableValue))
			{
				return Configuration.GetSetting(VariableValue);
			}
			else if(!string.IsNullOrEmpty(this.Value))
			{
				return this.Value;
			}
			else
			{
				return fi.GetValue(operation);
			}
		}

		// CONVERT THE VALUE FROM WIRE FOR RESPONSE MODEL
		//  applies converters to the raw value in order and outputs the converted value
		//  - split a string and join it
		//  - lowercase a string
		//  - encoding a string
		//  - escaping a string
		//  - deserialize text to json or xml
		//  - deserialize texture from bytes
		//  - deserialize sprite from bytes
		//  [someattribute(Converters = new type[] (typeof(LowerCaseConverter), typeof(Base64EncodeConverter))]
		//  [someattribute(Converters = new type[] (typeof(SpriteToBytesConverter))]
		//  [someattribute(Converters = new type[] (typeof(TextureToBytesConverter))]
		//  [someattribute(Converters = new type[] (typeof(JSONSerializeConverter))]
		//  [someattribute(Converters = new type[] (typeof(XMLSerializeConverter))]
		public virtual object OnResponseApplyConverters(object @value, HttpOperation operation, FieldInfo fi)
		{
			if(Converters != null)
			{
				foreach(Type converterType in Converters)
				{
					IValueConverter converterInstance = System.Activator.CreateInstance(converterType) as IValueConverter;
					
					if(converterInstance == null)
					{
						operation.Log ("(HttpMappedValueAttribute)(OnResponseApplyConverters) Converter '" + converterType.FullName + "' must implement IValueConverter!", LogSeverity.ERROR);
						continue;
					}
					else
					{
						bool success = false;
						@value = converterInstance.Convert(@value, fi, out success, null);
						
						if(success)
						{
							operation.Log ("(HttpMappedValueAttribute)(OnResponseApplyConverters) Converter '" + converterType.FullName + "' applied.  Output type is " + (@value==null ? "(unknown) because @value is null" : "'"+@value.GetType().FullName+"'") + ".", LogSeverity.VERBOSE);
						}
						else
						{
							operation.Log ("(HttpMappedValueAttribute)(OnResponseApplyConverters) Converter '" + converterType.FullName + "' failed!", LogSeverity.WARNING);
						}
					}
				}
			}
			
			return @value;
		}

		// ASSIGN TO RESPONSE
		//  assign the @value to response model
		//  - assign model
		//		- json
		//		- string
		//		- byte[]
		//		- strong type
		public virtual void OnResponseResolveModel(object @value, HttpOperation operation, FieldInfo fi)
		{
			@value = 
				(@value == null) 
				? (fi.FieldType.IsValueType) 
					? Activator.CreateInstance(fi.FieldType) 
					: (fi.FieldType.GetConstructor(Type.EmptyTypes) != null) //UNITY 501f1: due to deserialization failures we still prefer to have an instance of a ref type than null
						? Activator.CreateInstance(fi.FieldType) 
						: null
				: @value;
		    
			fi.SetValue(
				operation,
				Convert.ChangeType(@value,fi.FieldType)
			);
		}


		/* request */


		// GET THE ARTIFACT NAME
		//  resolve the name 
		//  name could be:
		//  - header name, 
		//  - query string name, 
		//  - uri template expression,
		//  - form field name
		//  it can come from the attribute, fieldInfo.name or a config variable
		// [SomeAttribute(Name = "staticName")]
		// [SomeAttribute(Name = "$uri-template-expression")]
		// [SomeAttribute()]
		// [SomeAttribute(VariableName = "config.some.name")]
		public virtual string OnRequestResolveName(HttpOperation operation, FieldInfo fi)
		{
			if(VariableName != null && Configuration.HasSetting(VariableName))
			{
				return Configuration.GetSetting<string>(VariableName);
			}
			else if(!string.IsNullOrEmpty(this.Name))
			{
				return this.Name;
			}
			else if(fi != null)
			{
				return fi.Name;
			}
			else
			{
				return null;
			}
		}

		// GET THE RAW VALUE
		//  value could come from the attribute, fieldInfo.getValue() or from a config variable
		// [SomeAttribute(Value = "staticValue")]
		// [SomeAttribute(Value = "some,array,of,values")]
		// [SomeAttribute()]
		// [SomeAttribute(VariableValue = "config.some.value")]
		public virtual object OnRequestResolveValue(string name, HttpOperation operation, FieldInfo fi)
		{
			if(VariableValue != null && Configuration.HasSetting(VariableValue))
			{
				return Configuration.GetSetting(VariableValue);
			}
			else if(!string.IsNullOrEmpty(this.Value))
			{
				return this.Value;
			}
			else if(fi != null)
			{
				return fi.GetValue(operation);
			}
			else
			{
				return null;
			}
		}

		// CONVERT THE VALUE FOR WIRE
		//  applies converters to the raw value in order and outputs the converted value
		//  - split a string and join it
		//  - lowercase a string
		//  - encoding a string
		//  - escaping a string
		//  - serialize texture to bytes
		//  - serialize sprite to bytes
		//  [someattribute(Converters = new type[] (typeof(LowerCaseConverter), typeof(Base64EncodeConverter))]
		//  [someattribute(Converters = new type[] (typeof(SpriteToBytesConverter))]
		//  [someattribute(Converters = new type[] (typeof(TextureToBytesConverter))]
		//  [someattribute(Converters = new type[] (typeof(JSONSerializeConverter))]
		//  [someattribute(Converters = new type[] (typeof(XMLSerializeConverter))]
		public virtual object OnRequestApplyConverters(object @value, HttpOperation operation, FieldInfo fi)
		{
			if(Converters != null)
			{
				foreach(Type converterType in Converters)
				{
					IValueConverter converterInstance = System.Activator.CreateInstance(converterType) as IValueConverter;

					if(converterInstance == null)
					{
						operation.Log ("(HttpMappedValueAttribute)(OnRequestApplyConverters) Converter '" + converterType.FullName + "' must implement IValueConverter!", LogSeverity.ERROR);
						continue;
					}
					else
					{
						bool success = false;
						@value = converterInstance.Convert(@value, fi, out success, null);

						if(success)
						{
							operation.Log ("(HttpMappedValueAttribute)(OnRequestApplyConverters) Converter '" + converterType.FullName + "' applied.", LogSeverity.VERBOSE);
						}
						else
						{
							operation.Log ("(HttpMappedValueAttribute)(OnRequestApplyConverters) Converter '" + converterType.FullName + "' failed!", LogSeverity.WARNING);
						}
					}
				}
			}

			return @value;
		}

		// ASSIGN FOR WIRE
		//  assign the @value to the request-model, where appropriate
		//  - push into headers hash
		//  - push into query string hash
		//  - push into uri template expressions hash
		//  - assign body
		//		- json
		//		- string
		//		- byte[]
		public virtual void OnRequestResolveModel(string name, object @value, ref HttpRequestModel model, HttpOperation operation, FieldInfo fi)
		{
			
		}
	}
}
