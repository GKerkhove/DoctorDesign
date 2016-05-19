
using System;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

using hg.ApiWebKit.core.http;
using System.Collections.Generic;
using System.Linq;

namespace hg.ApiWebKit.apis.google.editor
{
	public class ApiDirectoryEditor : EditorWindow
	{
		public string WindowTitle = "Google API Directory";
		//public Vector2 WindowSizeMin = new Vector2(700,600);
		//public Vector2 WindowSizeMax = new Vector2(700,600);
	
		#region Window Lifecycle
		void OnEnable()
		{

		}
		
		void OnDestroy()
		{

		}
		
		public ApiDirectoryEditor()
		{
			this.title = this.WindowTitle;
			//this.minSize = this.WindowSizeMin;
			//this.maxSize = this.WindowSizeMax;
		
			ctor();
		}
	
		public static ApiDirectoryEditor Init()
		{
			ApiDirectoryEditor window = EditorWindow.GetWindow<ApiDirectoryEditor>();
			window.Show();
			return window;
		}
		#endregion
		
		#region Window States
		private enum EnumFlowState
		{
			STARTING,
			API_DIR_RETRIEVING,
			API_DIR_SUCCESS,
			API_DIR_FAILURE
		}
		
		private EnumFlowState _currentFlowState = EnumFlowState.STARTING;
		private EnumFlowState _previousFlowState = EnumFlowState.STARTING;
		
		private EnumFlowState changeState(EnumFlowState newState)
		{
			if(_currentFlowState == newState)
				return _currentFlowState;
			
			_previousFlowState = _currentFlowState;
			_currentFlowState = newState;
			
			Repaint();
			
			return _currentFlowState;
		}
		#endregion
		
		models.ApiDiscoveryDirectoryList _apiDirectory;
		
		void retrieveApiDirectory()
		{
			new google.operations.GetApiDirectory()
				.Send(GetApiDirectoryOnSuccess,GetApiDirectoryOnFailure,GetApiDirectoryOnComplete);

			changeState(EnumFlowState.API_DIR_RETRIEVING);
		}
		
		int _imageOpsStarted=0;
		int _imageOpsFinished=0;
		
		private void GetApiDirectoryOnSuccess(google.operations.GetApiDirectory operation, HttpResponse response)
		{
			_apiDirectory = operation.Directory;
			changeState(EnumFlowState.API_DIR_SUCCESS);
			
			foreach(models.ApiDiscoveryDirectoryItem api in _apiDirectory.items)
			{
				api.Image = new Texture2D(32,32);
			
				if(api.preferred)
				{
					new hg.ApiWebKit.apis.example.media.operations.GetImage {
						ImageUri = api.icons.x32
					}
					.Send(OnImageRequestStart, OnImageResponseSuccess, OnImageResponseFail, OnImageResponseComplete, api.id);
				}
			}
		}

		private void GetApiDirectoryOnFailure(google.operations.GetApiDirectory operation, HttpResponse response)
		{
			changeState(EnumFlowState.API_DIR_FAILURE);
		}
		
		private void GetApiDirectoryOnComplete(google.operations.GetApiDirectory operation, HttpResponse response)
		{
			
		}
		
		
		
		private void OnImageRequestStart(hg.ApiWebKit.apis.example.media.operations.GetImage operation, HttpRequest request)
		{
			_imageOpsStarted++;
		}
		
		private void OnImageResponseSuccess(hg.ApiWebKit.apis.example.media.operations.GetImage operation, HttpResponse response)
		{
			_apiDirectory.items.ToList().Find(x => x.id == operation.ExtraParameters[0]).Image = operation.ImageTexture;
		}
		
		private void OnImageResponseFail(hg.ApiWebKit.apis.example.media.operations.GetImage operation, HttpResponse response)
		{

		}
		
		private void OnImageResponseComplete(hg.ApiWebKit.apis.example.media.operations.GetImage operation, HttpResponse response)
		{
			_imageOpsFinished++;
			Repaint();
		}
		
		
		
		private void ctor()
		{
			_style1 = new GUIStyle();
			_style1.normal.textColor = Color.cyan;
			_style1.fontSize = 12;
			_style1.fontStyle = FontStyle.Bold;
			_style1.wordWrap = true;
			
			_style2 = new GUIStyle();
			_style2.normal.textColor = Color.green;
			
			_style3 = new GUIStyle();
			_style3.normal.textColor = Color.grey;
			
			_style4 = new GUIStyle();
			_style4.normal.textColor = Color.yellow;
			_style4.fontStyle = FontStyle.Italic;
			_style4.fontSize = 9;
			
			//Configuration.SetSetting("log-VERBOSE", false);
		}
		
		private Vector2 _scrollPosition;
		
		private GUIStyle _style1 = null;
		private GUIStyle _style2 = null;
		private GUIStyle _style3 = null;
		private GUIStyle _style4 = null;
		
		void Update()
		{
			if(_imageOpsFinished == _imageOpsStarted)
				Repaint();

			switch(_currentFlowState)
			{
			case EnumFlowState.STARTING:
				retrieveApiDirectory();
				break;
				
			case EnumFlowState.API_DIR_RETRIEVING:
			case EnumFlowState.API_DIR_FAILURE:
			case EnumFlowState.API_DIR_SUCCESS:
				break;
			}
		}
		
		private void OnGUI()
		{
			switch(_currentFlowState)
			{
			case EnumFlowState.STARTING:
			case EnumFlowState.API_DIR_RETRIEVING:
				EditorGUILayout.HelpBox("Wait...",MessageType.Info);
				break;
				
			case EnumFlowState.API_DIR_SUCCESS:
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Google API Directory");
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.BeginHorizontal();

				_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition,false,true);
				
				foreach(models.ApiDiscoveryDirectoryItem api in _apiDirectory.items)
				{
					if(api.preferred)
					{
						EditorGUILayout.Space();
						
						EditorGUILayout.BeginHorizontal();
						
						EditorGUILayout.LabelField(new GUIContent(api.Image), GUILayout.Width(32), GUILayout.Height(32));
						
						EditorGUILayout.BeginVertical();
						
						EditorGUILayout.BeginHorizontal();
						
						EditorGUILayout.LabelField(api.title, _style1, GUILayout.Width(_style1.CalcSize(new GUIContent(api.title)).x));
						
						//TODO: show label with an oval background like Unity asset labels
						if(api.labels != null)
						{
							foreach(string label in api.labels)
							{
								EditorGUILayout.LabelField(label, _style4);
							}
						}
						
						GUILayout.FlexibleSpace();
						
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.LabelField(api.description);
						EditorGUILayout.LabelField("Preferred Version: " + api.version, _style2);
						
						List<models.ApiDiscoveryDirectoryItem> otherVersions = _apiDirectory.items.ToList().FindAll(x => x.name == api.name && x.version != api.version);
						if(otherVersions.Count>0)
							EditorGUILayout.LabelField("Other Versions: " +  string.Join(", ", otherVersions.Select(x => x.version).ToArray()), _style3);
						
						EditorGUILayout.BeginHorizontal();
						
						if(GUILayout.Button("Docs",GUILayout.Width(100)))
						{
							Application.OpenURL(api.documentationLink);
						}
						if(GUILayout.Button("Discover",GUILayout.Width(100)))
						{
							Application.OpenURL(api.discoveryRestUrl);
						}
						if(GUILayout.Button("OAuth Scopes",GUILayout.Width(100)))
						{
							Application.OpenURL(api.discoveryRestUrl + "?fields=auth(oauth2(scopes))");
						}
						
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.EndVertical();
						
						EditorGUILayout.EndHorizontal();
					}
				}
				
				EditorGUILayout.EndScrollView();
				
				EditorGUILayout.EndHorizontal();
				
				
				break;
				
			case EnumFlowState.API_DIR_FAILURE:
				EditorGUILayout.HelpBox("Failed!",MessageType.Info);
				
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Backup API Discovery Check",GUILayout.Width(300)))
				{
					Application.OpenURL("https://discovery-check.appspot.com/");
				}
				EditorGUILayout.EndHorizontal();
				
				break;
			}
		
			//if (GUI.changed)
				Repaint();
		}
	}
}