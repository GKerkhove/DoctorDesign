using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using hg.ApiWebKit.apis.haptix.models;
using hg.ApiWebKit.tinyfsm;

namespace hg.ApiWebKit.apis.haptix.editor
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class HaptixEditorWindowTitle: Attribute
	{
		public string Title = "Window";
		
		public HaptixEditorWindowTitle(string title)
		{
			Title = title;
		}
	}
}
