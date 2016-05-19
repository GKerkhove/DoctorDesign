using hg.LitJson;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hg.ApiWebKit.apis.zebra.models
{
	[Serializable]
	public class ZebraConfigurationPoint
	{
		public override string ToString ()
		{
			return "Access: " + (CanRead ? "R" : "") + (CanWrite ? "W" : "") + "\r\n" +
				"Type: " + Type.ToString() + "\r\n" +
				"Range: " + string.Join(",",Range) + "\r\n" +
				"Default: " + (Default == null ? "(null)" : Default);
		}

		public ZebraConfigurationPoint(ZebraConfigurationCategory category, string fullName, JsonData jd)
		{
			Category = category;

			FullName = fullName;

			determinePath(fullName);
			determineType(jd);
			determineAccess(jd);
			determineClone(jd);
			determineArchive(jd);
			determineDefault(jd);

			determineRange(jd);

			setFields();
		}

		private void determinePath(string fullName)
		{
			Path = fullName.Split(new[]{'.'},StringSplitOptions.RemoveEmptyEntries);
			ShortName = Path[Path.Length-1];
		}

		private void determineType(JsonData jd)
		{
			try
			{
				switch(((string)jd["type"]).ToLower())
				{
				case "bool":
					Type = ZebraConfigurationPointType.BOOLEAN;
					break;
					
				case "string":
					Type = ZebraConfigurationPointType.STRING;
					break;
					
				case "integer":
					Type = ZebraConfigurationPointType.INTEGER;
					break;
					
				case "double":
					Type = ZebraConfigurationPointType.DOUBLE;
					break;
					
				case "enum":
					Type = ZebraConfigurationPointType.ENUMERATION;
					break;
					
				case "ipv4-address":
					Type = ZebraConfigurationPointType.IPV4_ADDRESS;
					break;
					
				default:
					Type = ZebraConfigurationPointType.UNKNOWN;
					break;
				}
			}
			catch(Exception ex)
			{
				Type = ZebraConfigurationPointType.UNKNOWN;
			}
		}
		
		private void determineAccess(JsonData jd)
		{
			try
			{
				string access = ((string)jd["access"]).ToLower();
				
				if(access.Contains("r")) 
					CanRead = true;
				
				if(access.Contains("w")) 
					CanWrite = true;
				
			}
			catch(Exception ex)
			{
				CanRead = true;
				CanWrite = false;
			}
		}
		
		private void determineClone(JsonData jd)
		{
			try
			{
				CanClone = (bool)jd["clone"];
			}
			catch(Exception ex)
			{
				CanClone = false;
			}
		}
		
		private void determineArchive(JsonData jd)
		{
			try
			{
				CanArchive = (bool)jd["archive"];
			}
			catch(Exception ex)
			{
				CanArchive = false;
			}
		}
		
		private void determineDefault(JsonData jd)
		{
			try
			{
				Default = jd["default"] == null ? null : (string)jd["default"];
			}
			catch(Exception ex)
			{
				Default = null;
			}
		}

		private void determineRange(JsonData jd)
		{
			try
			{
				string r = (string)jd["range"];

				switch(Type)
				{
					case ZebraConfigurationPointType.ENUMERATION:
					case ZebraConfigurationPointType.BOOLEAN:
						Range = r.Split(',').Select(p => p.Trim()).ToArray();
						break;

					case ZebraConfigurationPointType.STRING:
					case ZebraConfigurationPointType.INTEGER:
					case ZebraConfigurationPointType.DOUBLE:
					case ZebraConfigurationPointType.IPV4_ADDRESS:
						Range = r.Split('-').Select(p => p.Trim()).ToArray();
						break;
				}
			}
			catch(Exception ex)
			{
				Range = null;
			}
		}

		public ZebraConfigurationCategory Category
		{
			get;
			private set;
		}

		public string[] Path
		{
			get;
			private set; 
		}

		public string FullName
		{
			get;
			private set;
		}

		public string ShortName
		{
			get;
			private set;
		}

		public ZebraConfigurationPointType Type
		{
			get;
			private set;
		}
		
		public bool CanRead
		{
			get;
			private set;
		}
		
		public bool CanWrite
		{
			get;
			private set;
		}
		
		public bool CanClone
		{
			get;
			private set;
		}
		
		public bool CanArchive
		{
			get;
			private set;
		}
		
		public string Default
		{
			get;
			private set;
		}

		public string[] Range
		{
			get;
			private set;
		}

		void setFields()
		{
			_category = Category;
			_path = Path;
			_fullName = FullName;
			_shortName = ShortName;
			_range = Range;
			_type = Type;
			_canRead = CanRead;
			_canWrite = CanWrite;
			_canClone = CanClone;
			_canArchive = CanArchive;
			_default = Default;
		}

		[SerializeField]private ZebraConfigurationCategory _category;
		[SerializeField]private string[] _path;
		[SerializeField]private string _fullName;
		[SerializeField]private string _shortName;
		[SerializeField]private string[] _range;
		[SerializeField]private ZebraConfigurationPointType _type;
		[SerializeField]private bool _canRead;
		[SerializeField]private bool _canWrite;
		[SerializeField]private bool _canClone;
		[SerializeField]private bool _canArchive;
		[SerializeField]public string _default;
	}
}