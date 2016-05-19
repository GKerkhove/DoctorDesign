using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;

namespace hg.ApiWebKit.converters
{
	public class DeserializeSoap : IValueConverter
	{
		public object Convert (object input, FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				object instance = Activator.CreateInstance(targetField.FieldType);
				object result = deserializeSoap(instance as SoapMessage, targetField, (string)input);

				successful = true;

				return result;
			}
			catch (Exception ex)
			{
				Configuration.Log("(DeserializeSoap)(Convert) Failure : " + ex.Message, LogSeverity.ERROR);
				if(ex.InnerException!=null)
					Configuration.Log("(DeserializeSoap)(Convert) Failure-Inner : " + ex.InnerException.Message, LogSeverity.ERROR);
				return null;
			}
		}

		private object deserializeSoap<T>(T targetInstance, FieldInfo targetField, string xml) where T : SoapMessage
		{
			SoapMessageAttribute[] soapMsgAtt = (SoapMessageAttribute[])targetInstance.GetType().GetCustomAttributes(typeof(SoapMessageAttribute), false);
			
			if(soapMsgAtt.Length == 0)
				throw new NotSupportedException("SOAP Message must be decorated with SoapMessageAttribute");
			
			if(string.IsNullOrEmpty(soapMsgAtt[0].ElementName) || string.IsNullOrEmpty(soapMsgAtt[0].NamespaceUri))
				throw new NotSupportedException("SOAP Message SoapMessageAttribute declaration must specify ElementName and NamespaceUri.");
			
			models.SoapEnvelope<T> response = new models.SoapEnvelope<T>(targetInstance);
			
			XmlAttributeOverrides overrides = messageOverrides(response.Body.GetType(), targetInstance.GetType(), soapMsgAtt[0].ElementName, soapMsgAtt[0].NamespaceUri);

			XmlSerializer serializer = new XmlSerializer(response.GetType(), overrides);

			StringReader sr = new StringReader(xml);
			SoapEnvelope<T> result = (SoapEnvelope<T>) serializer.Deserialize(sr);

			sr.Close();

			return result.Body.Message;
		}
		
		private XmlElementAttribute messageElementModifier(Type messageType, string name, string namespaceUri)
		{
			XmlElementAttribute elementNameMod = new XmlElementAttribute(name, messageType);
			elementNameMod.Namespace = namespaceUri;
			
			return elementNameMod;
		}
		
		private XmlAttributeOverrides messageOverrides(Type overridenType, Type messageType, string elementName, string elementNamespace)
		{
			XmlElementAttribute msgAtt = messageElementModifier(messageType, elementName, elementNamespace);
			
			XmlAttributes overrideAtts = new XmlAttributes();
			overrideAtts.XmlElements.Add (msgAtt);
			
			XmlAttributeOverrides overrides = new XmlAttributeOverrides();
			overrides.Add (overridenType, "Message", overrideAtts);
			
			return overrides;
		}
	}
}

