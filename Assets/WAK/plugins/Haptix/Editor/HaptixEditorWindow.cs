using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using hg.ApiWebKit.apis.haptix.models;
using hg.ApiWebKit.tinyfsm;

namespace hg.ApiWebKit.apis.haptix.editor
{
	public abstract class HaptixEditorWindow : EditorWindow, IHasCustomMenu
	{
		public static HaptixEditorWindow Instance { get; private set; }
		
		public static void ForceClose()
		{
			//Debug.Log ("FORCE_CLOSE editor window instance " + HaptixEditorWindow.Instance.ToString());
		
			if(HaptixEditorWindow.IsOpen)
				HaptixEditorWindow.Instance.Close();
		}
		
		public static bool IsOpen {
			get { return Instance != null; }
		}
	
		/*
		//TODO: implement with reflection
		public static T ReOpen<T>()
		{
			if(HaptixEditorWindow.IsOpen)
			{
				EditorWindow.FocusWindowIfItsOpen<HaptixEditorWindow>();
				return null;
			}
			else
				return HaptixEditorWindow.Show();
		}
		*/
		
		public static T Show<T>() where T : HaptixEditorWindow
		{
			T window = EditorWindow.GetWindow<T>();
			window.Show();
			return window;
		}
		
		//HACK: when this window is visible and DLL is recompiled then OnEnable deserializes stored data, such as the 24hr-logo
		//		we can check this data to see which state we can go into
		//		since DLL recompile will not call Open again
		protected bool Serialized = false;
		
		protected TinyStateMachine FSM = null;
	
		protected virtual void OnEnableUnserialized()
		{
		
		}
	
		protected virtual void OnEnableSerialized()
		{
		
		}
	
		protected virtual void OnEnable()
		{
			//Debug.LogError("window:onEnable, instance null: " + (Instance == null).ToString());
			
			Instance = this;
			
			FSM = new TinyStateMachine(this,OnStateChange);
			
			if(Serialized)
				OnEnableSerialized();
			else
				OnEnableUnserialized();
			
			Serialized = true;
		}
	
		protected virtual void OnStateChange(TinyStateMachine fsm, string previous, string current)
		{
			
		}
	
		protected virtual void OnDisable()
		{
			//Debug.LogError("window:onDisable");
		
			FSM.Stop(true);
			
			Instance = null;
		}

		protected virtual void OnDestroy()
		{
			//Debug.LogError("window:onDestroy");
		}
	
		protected virtual void Update()
		{
			FSM.Update();
		}

		protected virtual void OnGUI()
		{
			FSM.OnGUI();
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			
		}
		
		public HaptixEditorWindow()
		{
			//Debug.LogError("window:constructor");
			
			Constructor();
		}

		protected virtual void Constructor()
		{
			HaptixEditorWindowTitle[] titles = (HaptixEditorWindowTitle[])this.GetType().GetCustomAttributes(typeof(HaptixEditorWindowTitle), true);
			if(titles.Length>0)
				this.title = titles[0].Title;
			else 
				this.title = "Window";

			this.wantsMouseMove = true;
		}
	}
}
