using hg.LitJson;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using hg.ApiWebKit.core.tcp;

namespace hg.ApiWebKit.apis.zebra
{
	public class ZebraConfigurator: TcpOperation
	{
		private enum UiState
		{
			BROWSING,
			OPTIONS,
			POINT,
			POINT_READING,
			POINT_WRITING,
			QUICK_QUERY
		}

		models.ZebraConfigurations _configurations = new models.ZebraConfigurations();

		[SerializeField]List<models.ZebraConfigurationPoint> _writeables;
		[SerializeField]List<models.ZebraConfigurationPoint> _readables;
		[SerializeField]List<models.ZebraConfigurationCategory> _topLevelCategories;

		private string _topLevelName = "Zebra Zombies";
		private bool _showDetails = false;
		private models.ZebraConfigurationCategory _browsingCategory = null;
		private List<models.ZebraConfigurationCategory> _categoriesOnDisplay;
		private List<models.ZebraConfigurationPoint> _pointsOnDisplay;
		private models.ZebraConfigurationPoint _selectedPoint = null;
		private string _selectedPointValue = "";
		private string _selectedPointMessage = "Ready.";
		private string _quickQueryValue = "?";


		void Awake()
		{
			Configuration.SetSetting("log-VERBOSE",true);

			_configurations.Load();

			//_writeables = _configurations.AllWriteablePoints().Values.ToList();
			//_readables = _configurations.AllReadablePoints().Values.ToList();

			_topLevelCategories = _configurations.TopLevelCategories().Values.ToList();
			_categoriesOnDisplay = _topLevelCategories;
			_pointsOnDisplay = _configurations.ConfigurablePoints(null).Values.ToList();
		}

		void Start()
		{

		}

		private Rect _fullScreenRect = new Rect(0,0,Screen.width,Screen.height);

		[SerializeField]
		private GUISkin _skin;

		private Vector2 _scrollPosition = Vector2.zero;
		private bool _isScrolling = false;
		private UiState _state = UiState.BROWSING;

		void Update()
		{
			foreach(Touch touch in Input.touches) 
			{
				if (touch.phase == TouchPhase.Moved)
				{
					_isScrolling = true;
					_scrollPosition.y += touch.deltaPosition.y;
				}
				else
					_isScrolling = false;
			}
		}

		void OnGUI()
		{
			if(GUI.skin != _skin)
				GUI.skin = _skin;

			if(_state==UiState.BROWSING || _state==UiState.QUICK_QUERY)
			{
				doBrowse();
			}
			else if(_state==UiState.OPTIONS)
			{
				doOptions();
			}
			else if(_state==UiState.POINT)
			{
				doPoint();
			}
			else 
			{
				GUILayout.Label("WAIT...",GUILayout.Width(Screen.width));
			}

		}

		void doPoint()
		{
			GUILayout.BeginArea(_fullScreenRect);
			
			GUILayout.BeginVertical();
			
			GUILayout.Label("point : " + _selectedPoint.FullName, GUILayout.Width(Screen.width));
			GUILayout.Space(30);
			
			GUILayout.Label(_selectedPoint.ToString());
			GUILayout.Space(30);

			GUILayout.Label(_selectedPointMessage);
			_selectedPointValue = GUILayout.TextField(_selectedPointValue);

			GUILayout.BeginHorizontal();

			if(_selectedPoint.CanRead)
			{
				if(GUILayout.Button("Read", GUILayout.Width(Screen.width/2)))
				{
					_state = UiState.POINT_READING;
					Send(null,null);
				}
			}

			if(_selectedPoint.CanWrite)
			{
				if(GUILayout.Button("Write", GUILayout.Width(Screen.width/2)))
				{
					_state = UiState.POINT_WRITING;
					Send(null,null);
				}
			}
			
			GUILayout.EndHorizontal();

			GUILayout.Space(100);

			if(GUILayout.Button("< back", GUILayout.Width(Screen.width/2)))
			{
				_selectedPoint = null;
				_selectedPointValue = "";
				_selectedPointMessage = "Ready.";
				_state=UiState.BROWSING;
			}


			GUILayout.EndArea();
		}

		void doOptions()
		{
			GUILayout.BeginArea(_fullScreenRect);
			
			GUILayout.BeginVertical();
			
			GUILayout.Label("Options");
			GUILayout.Space(30);

			GUILayout.Label("Hostname: ");
			OperationSettings.Hostname = GUILayout.TextField(OperationSettings.Hostname);

			GUILayout.Label("Json Port: ");
			OperationSettings.Port = int.Parse(GUILayout.TextField(OperationSettings.Port.ToString()));

			if(GUILayout.Button("OK", GUILayout.Width(Screen.width/2)))
			{
				_state=UiState.BROWSING;
			}

			GUILayout.EndArea();
		}

		void doBrowse()
		{
			GUILayout.BeginArea(_fullScreenRect);

			GUILayout.BeginVertical();
			
			GUILayout.Label(_browsingCategory==null?_topLevelName:"category : "+_browsingCategory.FullName);
			GUILayout.Space(30);
			
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(_fullScreenRect.width));
			
			foreach(models.ZebraConfigurationCategory category in _categoriesOnDisplay)
			{
				if(GUILayout.Button("["+category.Name+"]",GUILayout.Width(Screen.width - 230)))
				{
					browseCategory(category.Id);
				}
			}
			
			if(_pointsOnDisplay!= null)
			{
				GUILayout.Space(30);
				
				foreach(models.ZebraConfigurationPoint point in _pointsOnDisplay)
				{
					if(GUILayout.Button("<"+point.ShortName+">",GUILayout.Width(Screen.width - 230)))
					{
						_selectedPoint = point;
						_state = UiState.POINT;
					}

					if(_showDetails)
					{
						GUILayout.Label(point.ToString());
						/*GUILayout.BeginHorizontal();
						GUILayout.Label(_quickQueryValue);
						if(GUILayout.Button("Query"))
						{
							_state=UiState.QUICK_QUERY;

						}
						GUILayout.EndHorizontal();*/
						GUILayout.Space(30);
					}
				}
			}
			
			GUILayout.EndScrollView();
			
			GUILayout.EndVertical();
			
			GUILayout.BeginHorizontal();
			
			if(_browsingCategory!=null)
			{
				if(GUILayout.Button("< previous", GUILayout.Width(Screen.width/2)))
				{
					goUpOneCategory();
				}
			}
			else
			{
				if(GUILayout.Button("Target", GUILayout.Width(Screen.width/2)))
				{
					_state=UiState.OPTIONS;
				}
			}
			
			if(GUILayout.Button("Toggle Details", GUILayout.Width(Screen.width/2)))
			{
				_showDetails = !_showDetails;
			}
			
			GUILayout.EndHorizontal();
			
			GUILayout.EndArea();
		}

		void goUpOneCategory()
		{
			models.ZebraConfigurationCategory parent = _configurations.ParentCategory(_browsingCategory==null?null:_browsingCategory.Id);

			if(parent == null)
			{
				_categoriesOnDisplay = _topLevelCategories;
				_pointsOnDisplay = _configurations.ConfigurablePoints(null).Values.ToList();
				_browsingCategory = null;
			}
			else
			{
				browseCategory(parent.Id);
			}
		}

		void browseCategory(string id)
		{
			if(_isScrolling) return;

			models.ZebraConfigurationCategory category = _configurations.Category(id);
			_categoriesOnDisplay = _configurations.ChildCategories(id).Values.ToList();
			_pointsOnDisplay = _configurations.ConfigurablePoints(id).Values.ToList();
			_browsingCategory = category;
		}

		#region Tcp Socket callbacks
		// Failure received from Tcp Provider
		protected override void OnFailure (Exception exception)
		{
			_selectedPointMessage = "ERROR:" + exception.Message;
			_state = UiState.POINT;
		}
		
		
		// Invoked from base after user invokes the Send method.
		//  The return value will be used as the data to send over Tcp Socket
		protected override string OnRequest ()
		{
			string v = "null";

			if(_state==UiState.POINT_WRITING)
				v = "\"" + _selectedPointValue.Trim() + "\"";

			return "{}{\"" + _selectedPoint.FullName + "\":" + v + "}";
		}
		
		// Invoked from base on a successful response
		//  The message parameter is the data received from Tcp Socket in full
		protected override void OnResponse (string message)
		{
			try
			{
				JsonData jd = hg.LitJson.JsonMapper.ToObject(message);
				_selectedPointValue = (string)jd[_selectedPoint.FullName];
				_selectedPointMessage = "Response: '" + _selectedPointValue + "'";
				_state = UiState.POINT;
			}
			catch (Exception ex)
			{
				_selectedPointValue = "?";
				_selectedPointMessage = "ERROR: " + ex.Message;
				_state = UiState.POINT;
			}
		}
		#endregion
	}
}