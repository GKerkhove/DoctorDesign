using UnityEngine;
using System;

using System.Xml;
using System.Xml.Serialization;

namespace hg.ApiWebKit.models
{
	public class SoapBody<T> where T : SoapMessage
	{
		public SoapBody() {}

		public SoapBody(T message)
		{
			Message = message;
		}

		public T Message;
	}
}