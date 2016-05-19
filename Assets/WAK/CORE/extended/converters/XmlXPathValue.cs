using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using hg.ApiWebKit.models;
using hg.ApiWebKit.attributes;
using hg.ApiWebKit.mappers;

namespace hg.ApiWebKit.converters
{
	public class XmlXPathValue : IValueConverter
	{
		public object Convert (object input, FieldInfo targetField, out bool successful, params object[] parameters)
		{ 
			successful = false;

			if(input == null)
				return null;

			try
			{
				HttpResponseXmlBodyXPathValueAttribute[] xpathAtts = (HttpResponseXmlBodyXPathValueAttribute[])targetField.GetCustomAttributes(typeof(HttpResponseXmlBodyXPathValueAttribute), false);
				string expression = xpathAtts[0].XPathExpression;
				
				string inputAsString = (string)input;
				inputAsString = inputAsString.Replace("&", "_"); //MONO BUG
				
				//TODO: more bugs
				
				
				XPathDocument document = new XPathDocument(new StringReader(inputAsString));
				string result = document.CreateNavigator().SelectSingleNode(expression).Value;
				
				successful = true;

				return result;
			}
			catch (Exception ex)
			{
				Configuration.Log("(XmlXpathValue)(Convert) Failure : " + ex.Message, LogSeverity.ERROR);
				if(ex.InnerException!=null)
					Configuration.Log("(XmlXpathValue)(Convert) Failure-Inner : " + ex.InnerException.Message, LogSeverity.ERROR);
				return null;
			}
		}
	}
}

