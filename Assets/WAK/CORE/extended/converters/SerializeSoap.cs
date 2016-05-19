using UnityEngine;
using System;
using System.Text;
using hg.ApiWebKit.attributes;
using hg.ApiWebKit.models;
using System.Xml.Serialization;
using System.IO;

namespace hg.ApiWebKit.converters
{
	public class SerializeSoap : IValueConverter
	{
		public object Convert(object input, System.Reflection.FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				string xmlString = serializeSoap(input as SoapMessage);
				successful = true;
				return xmlString;
			}
			catch 
			{
				return null;
			}
		}

		private sealed class Utf8StringWriter : StringWriter
		{
			public override Encoding Encoding { get { return Encoding.UTF8;  } } 
		}

		private string serializeSoap<T>(T messageInstance) where T: SoapMessage
		{
			if(messageInstance == null)
				throw new NotSupportedException("Message instance can not be empty.");
			
			SoapMessageAttribute[] soapMsgAtt = (SoapMessageAttribute[])messageInstance.GetType().GetCustomAttributes(typeof(SoapMessageAttribute), false);
			
			if(soapMsgAtt.Length == 0)
				throw new NotSupportedException("SOAP Message must be decorated with SoapMessageAttribute");
			
			if(string.IsNullOrEmpty(soapMsgAtt[0].ElementName) || string.IsNullOrEmpty(soapMsgAtt[0].NamespacePrefix) || string.IsNullOrEmpty(soapMsgAtt[0].NamespaceUri))
				throw new NotSupportedException("SOAP Message SoapMessageAttribute declaration must specify ElementName, NamespacePrefix and NamespaceUri.");

			models.SoapEnvelope<T> request = new models.SoapEnvelope<T>(messageInstance);

			XmlAttributeOverrides overrides = messageOverrides(request.Body.GetType(), messageInstance.GetType(), soapMsgAtt[0].ElementName, soapMsgAtt[0].NamespaceUri);
			
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("soap", "http://schemas.xmlsoap.org/soap/envelope/");
			ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
			ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			ns.Add(soapMsgAtt[0].NamespacePrefix, soapMsgAtt[0].NamespaceUri);
			
			Utf8StringWriter sw = new Utf8StringWriter();
			XmlSerializer serializer = new XmlSerializer(request.GetType(), overrides);
			serializer.Serialize(sw, request, ns);
			
			string xml = sw.GetStringBuilder().ToString();

			sw.Close();

			return xml;
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

