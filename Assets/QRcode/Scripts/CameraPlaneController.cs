/// <summary>
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
