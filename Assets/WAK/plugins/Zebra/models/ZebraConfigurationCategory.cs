using hg.LitJson;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hg.ApiWebKit.apis.zebra.models
{
	[Serializable]
	public class ZebraConfigurationCategory
	{
		public ZebraConfigurationCategory(string id, string fullname, string name, ZebraConfigurationCategory parent)
		{
			Id = id;
			FullName = fullname;
			Name = name;
			Parent = parent;

			setFields();
		}

		public string Id
		{
			get;
			private set;
		}

		public string FullName
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public ZebraConfigurationCategory Parent
		{
			get;
			private set;
		}

		public bool IsTopLevel
		{
			get { return Parent == null; }
		}

		private void setFields()
		{
			_id = Id;
			_fullName = FullName;
			_name = Name;
			_isTopLevel = IsTopLevel;
			_parentId = Parent != null ? Parent.Id : "(null)";
			_parentName = Parent != null ? Parent.Name : "(null)";
		}

		[SerializeField]private string _id;
		[SerializeField]private string _name;
		[SerializeField]private string _fullName;
		[SerializeField]private bool _isTopLevel;
		[SerializeField]private string _parentId;
		[SerializeField]private string _parentName;
	}
}