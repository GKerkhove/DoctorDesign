
using System;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

using hg.ApiWebKit.core.http;
using System.Collections.Generic;
using System.Linq;
using hg.ApiWebKit.apis.mantisbt.models;


namespace hg.ApiWebKit.apis.mantisbt.editor
{
	public class SubmitIssueEditor : EditorWindow, IHasCustomMenu
	{
		private bool DEBUG = false;
		
		private void log(string message)
		{
			if(!DEBUG)
				return;
			
			Debug.Log("[SubmitIssueEditor] " + message);
		}


		private enum EnumFlowState
		{
			STARTING,
			REQUESTING_PROJECTS,
			SUBMIT_MANUAL,
			USER_INFO_COLLECT,
			USER_INFO_SUBMIT,
			ISSUE_WAIT,
			ISSUE_CREATED,
			ISSUE_NOTCREATED
		}

		private EnumFlowState _currentFlowState = EnumFlowState.STARTING;
		private EnumFlowState _previousFlowState = EnumFlowState.STARTING;

		private EnumFlowState changeState(EnumFlowState newState)
		{
			if(_currentFlowState == newState)
				return _currentFlowState;

			log ("State Change : " + _currentFlowState + " => " + newState);

			_previousFlowState = _currentFlowState;
			_currentFlowState = newState;

			Repaint();

			return _currentFlowState;
		}

		private bool _ready = false;

		private string _windowTitle = "Submit Issue";

		public void DebuggingToggle()
		{
			DEBUG = !DEBUG;
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(new GUIContent("Debugging"), DEBUG, new GenericMenu.MenuFunction(this.DebuggingToggle));
		}



		private string[] _projectNames = null;
		private int _selectedProject = 0;

		private string[] _issueSeverities = new string[] { "feature", "tweak" };
		private int _selectedSeverity = 0;

		private string _issueSummary = "";
		private string _issueDescription = "";
		private string _issueEmail = "";

		private string _lastSubmittedIssueId = "0";

		private List<ProjectData> _projects = new List<ProjectData>();
		
		private int findProjectId(string projectName)
		{
			ProjectData prj = _projects.Find(p => p.name == projectName);

			if(prj==null)
				return 0;
			else 
				return prj.id;
		}

		private int findProjectIdFromSelectedIndex()
		{
			string prjName = _projectNames[_selectedProject];
			return findProjectId(prjName);
		}



		private SubmitIssueEditor()
		{
			log("++.ctor");

			title = _windowTitle;
			minSize = new Vector2(300,520);

			Configuration.SetBaseUri("mantisbt","http://haptixgames.com/triage");
			Configuration.SetSetting("mantisbt.username","api");
			Configuration.SetSetting("mantisbt.password","api");
		}
		
		public static SubmitIssueEditor Init()
		{
			SubmitIssueEditor window = EditorWindow.GetWindow<SubmitIssueEditor>();
			window.Show();

			return window;
		}

		void Update()
		{
			switch(_currentFlowState)
			{
			case EnumFlowState.STARTING:
				requestProjects();
				changeState(EnumFlowState.REQUESTING_PROJECTS);

				break;

			case EnumFlowState.REQUESTING_PROJECTS:
				EditorUtility.SetDirty(Camera.main);
				break;

			case EnumFlowState.USER_INFO_COLLECT:

				break;

			case EnumFlowState.USER_INFO_SUBMIT:
				_lastSubmittedIssueId = "0";
				submitIssue();
				changeState(EnumFlowState.ISSUE_WAIT);

				break;

			case EnumFlowState.ISSUE_WAIT:
				EditorUtility.SetDirty(Camera.main);

				break;

			case EnumFlowState.ISSUE_CREATED:

				break;

			case EnumFlowState.ISSUE_NOTCREATED:

				break;
			
			}
		}

		private string _message = "Please fill out the form.";
		private MessageType _messageType = MessageType.Info;

		private void OnGUI()
		{
			switch(_currentFlowState)
			{
				case EnumFlowState.REQUESTING_PROJECTS:
					EditorGUILayout.HelpBox("Wait...",MessageType.Info);
					break;

				case EnumFlowState.SUBMIT_MANUAL:
					EditorGUILayout.HelpBox("Please submit your issue inside portal.",MessageType.Info);
					
					if(GUI.Button(new Rect(0, position.height - 50, position.width, 50), "Open Portal"))
					{
						Application.OpenURL(Configuration.GetBaseUri("mantisbt"));
					}
					
					break;

				case EnumFlowState.USER_INFO_COLLECT:
					
					_selectedProject = EditorGUI.Popup(new Rect(5,50,position.width - 20,20), "Select Project:", _selectedProject, _projectNames);

					_selectedSeverity = EditorGUI.Popup(new Rect(5,80,position.width - 20,20), "Select Severity:", _selectedSeverity, _issueSeverities);

					GUI.Label(new Rect(5,110,120,20), "Your Email:"); 
					GUI.SetNextControlName("email");
					_issueEmail = EditorGUI.TextField(new Rect(130,110, position.width-150, 20), _issueEmail);

					GUI.Label(new Rect(5,140,120,20), "Summary:"); 
					_issueSummary = EditorGUI.TextField(new Rect(130,140, position.width-150, 20), _issueSummary);

					GUI.Label(new Rect(5,170,120,20), "Description:"); 
					_issueDescription = EditorGUI.TextArea(new Rect(130,170, position.width-150, 200), _issueDescription);

					if(GUI.Button(new Rect(5, 400, 100, 40), "Reset Form"))
					{
						GUI.FocusControl("email");
						_issueSummary = "";
						_issueDescription = "";
					}

					if(GUI.Button(new Rect(position.width - 105, 400, 100, 40), "Open Portal"))
					{
						Application.OpenURL(Configuration.GetBaseUri("mantisbt"));
					}

					if(GUI.Button(new Rect(0, position.height - 50, position.width, 50), "Submit"))
					{
						if(string.IsNullOrEmpty(_issueSummary.Trim()) || string.IsNullOrEmpty(_issueDescription.Trim()))
						{
							_message = "Please fill out Summary and Description";
							_messageType = MessageType.Error;
						}
						else
						{
							_messageType = MessageType.None;
							changeState(EnumFlowState.USER_INFO_SUBMIT);
						}
					}

					EditorGUILayout.HelpBox(_message,_messageType);

					break;

				case EnumFlowState.ISSUE_WAIT:
					EditorGUILayout.HelpBox("Submitting...",MessageType.Info);

					break;
				case EnumFlowState.ISSUE_CREATED:
					EditorGUILayout.HelpBox("Issue Created!",MessageType.Info);
						
					if(GUI.Button(new Rect(5, 50, 100, 40), "View Issue"))
					{
						Application.OpenURL(Configuration.GetBaseUri("mantisbt") + "/view.php?id=" + _lastSubmittedIssueId);
					}

					if(GUI.Button(new Rect(0, position.height - 50, position.width, 50), "Submit Another"))
					{
						changeState(EnumFlowState.STARTING);
					}

					break;

				case EnumFlowState.ISSUE_NOTCREATED:
					EditorGUILayout.HelpBox("Issue NOT Created!",MessageType.Error);
					
					if(GUI.Button(new Rect(0, position.height - 50, position.width, 50), "Submit Again"))
					{
						changeState(EnumFlowState.STARTING);
					}

					break;

			}

			if (GUI.changed)
			{
				Repaint ();
			}
		}


		private void requestProjects()
		{
			new operations.GetAccessibleProjects {
				Request = new models.ProjectDataRequest {
					Username = Configuration.GetSetting<string>("mantisbt.username"),
					Password = Configuration.GetSetting<string>("mantisbt.password")
				}
			}
			.Send(
				new Action<operations.GetAccessibleProjects, HttpResponse> ((operation, response) => 
				{ 
					_projects.AddRange(operation.Response.Projects.Project);

					List<string> projectNames = new List<string>();

					int i = 0;

					foreach(var project in operation.Response.Projects.Project)
					{
						projectNames.Add (project.name);

						if(project.name == "Web API Kit")
							_selectedProject = i;

						i++;
					}
					
					_projectNames = projectNames.ToArray();

					changeState(EnumFlowState.USER_INFO_COLLECT);
					
					Repaint();
				}),
				new Action<operations.GetAccessibleProjects, HttpResponse> ((operation, response) => 
			    { 
					changeState(EnumFlowState.SUBMIT_MANUAL);
				}),
				null
			);
		}

		private void submitIssue()
		{
			new operations.AddIssue {
				Request = new models.AddIssueRequest {
					Username = Configuration.GetSetting<string>("mantisbt.username"),
					Password = Configuration.GetSetting<string>("mantisbt.password"),
					IssueData = new models.IssueData() {
						project = new models.ObjectRef {
							id = findProjectIdFromSelectedIndex().ToString()
						},
						summary = _issueSummary.Trim(),
						description = _issueDescription.Trim(),
						category = "General",
						additional_information = (string.IsNullOrEmpty(_issueEmail.Trim()) ? "No Contact Provided" : _issueEmail.Trim()),
						severity = new models.ObjectRef {
							name = _issueSeverities[_selectedSeverity]
						}
					}
				}
			}
			.Send(
				new Action<operations.AddIssue, HttpResponse> ((operation, response) => 
			    { 
					_lastSubmittedIssueId = operation.Response.IssueId;

					changeState(EnumFlowState.ISSUE_CREATED);
				
					Repaint();
				}),
				new Action<operations.AddIssue, HttpResponse> ((operation, response) => 
			    { 
					changeState(EnumFlowState.ISSUE_NOTCREATED);
				}),
				null
			);
		}
	}
}
