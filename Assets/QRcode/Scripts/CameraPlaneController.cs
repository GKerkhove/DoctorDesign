﻿/// <summary>
/// write by 52cwalk,if you have some question ,please contract lycwalk@gmail.com
/// </summary>

using UnityEngine;
using System.Collections;

public class CameraPlaneController : MonoBehaviour {

	public Camera _targetCam;

	ScreenOrientation orientation;
	float height = 0;
	float width = 0;

	int sdkVersion=0;
	void Start()
	{
		sdkVersion = GetSDKLevel ();

	}
	// Use this for initialization
	void Awake () {
		float Screenheight = (float)_targetCam.orthographicSize* 2.0f; 
 		float Screenwidth = Screenheight * Screen.width / Screen.height;
 		height = Screenheight ;
 		width = Screenwidth;
 		this.transform.localPosition = new Vector3(0,0,91.6f);
 
 		#if UNITY_EDITOR
 		transform.localEulerAngles = new Vector3(90,180,0);
 		transform.localScale = new Vector3(width/10, 1.0f, height/10);
 		#elif UNITY_WEBPLAYER
 		transform.localEulerAngles = new Vector3(90,180,0);
 		transform.localScale = new Vector3(width/10, 1.0f, height/10);
 		#endif
 
 		orientation = Screen.orientation;
 
 		Screen.sleepTimeout = SleepTimeout.NeverSleep;
 		if (Screen.orientation == ScreenOrientation.Portrait||
 		    Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
 
 			#if UNITY_ANDROID
 			transform.localEulerAngles = new Vector3(0,270,90);
 			transform.localScale = new Vector3(height/10, 1.0f, width/10);
 			#elif UNITY_IOS
 			if( Screen.orientation == ScreenOrientation.PortraitUpsideDown)
 			{
 				transform.localEulerAngles = new Vector3(0,270,90);
 			}
 			else
 		{
 				transform.localEulerAngles = new Vector3(0,90,270);
 			}
 			transform.localScale = new Vector3(-1*height/10, 1.0f, width/10);
 			#endif
 		} else if (Screen.orientation == ScreenOrientation.Landscape) {
 			#if UNITY_ANDROID
 			transform.localEulerAngles = new Vector3(90,180,0);
 			transform.localScale = new Vector3(width/10, 1.0f, height/10);
 			#elif UNITY_IOS
 			transform.localEulerAngles = new Vector3(-90,0,0);
 			transform.localScale = new Vector3(-1*width/10, 1.0f, height/10);
 			
 			#endif
 		}
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	// Update is called once per frame
//	void Update () {
//	}

	int GetSDKLevel()
	{
		System.IntPtr calssz = AndroidJNI.FindClass ("android/os/Build$VERSION");
		System.IntPtr fieldID  = AndroidJNI.GetStaticFieldID(calssz,"SDK_INT", "I");
		int sdkLevel = AndroidJNI.GetStaticIntField(calssz, fieldID);
		return sdkLevel;
	}

}
