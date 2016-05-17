/// <summary>
/// write by 52cwalk,if you have some question ,please contract lycwalk@gmail.com
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DeviceCameraController : MonoBehaviour {

	public enum CameraMode
	{
		FACE_C,
		DEFAULT_C,
		NONE
	}
	[HideInInspector]
	public WebCamTexture cameraTexture = null; 

	private bool isPlay = false;
	//public CameraMode e_CameraMode;
	GameObject e_CameraPlaneObj;
	int matIndex = 0;

	ScreenOrientation orientation;
	public bool isPlaying
	{
		get{
			return isPlay;
		}
	}
	// Use this for initialization  
	void Awake()  
	{  
        print("PRE CAMCON");
//        StartCoroutine(Start());
        StartCoroutine(CamCon());  
		e_CameraPlaneObj = transform.FindChild ("CameraPlane").gameObject;
        print("AFER AWAKE");
	}
	
	// Update is called once per frame  
	void Update()  
	{  
		if (isPlay) {  
			if(e_CameraPlaneObj.activeSelf)
			{
				e_CameraPlaneObj.GetComponent<Renderer>().material.mainTexture = cameraTexture;
			}

		}
	
	}

    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            print("Has autho");
        }
        else
        {
            print("No autho");
        }
    }

	IEnumerator CamCon()
	{
        print(WebCamTexture.devices.Length);
        print("PRE AUTHO");
	    print(Application.RequestUserAuthorization(UserAuthorization.WebCam));
        print("IN CAMCON");
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);  
        print("AFTER YIELD");
		if (Application.HasUserAuthorization(UserAuthorization.WebCam))  
		{  
			#if UNITY_IOS
			if(Mathf.Min(Screen.width,Screen.height)>1000)
			{
				cameraTexture = new WebCamTexture(Screen.width/2,Screen.height/2);  
			}
//			else
//			{
//				cameraTexture = new WebCamTexture();  
//			}
		
//			#elif UNITY_ANDROID
			#endif
		    if (cameraTexture == null)
		    {
                WebCamDevice[] devices = WebCamTexture.devices;

                foreach (WebCamDevice cam in devices)
                {
                    if (!cam.isFrontFacing)
                    {
                        cameraTexture = new WebCamTexture(cam.name);
                        cameraTexture.deviceName = cam.name;
                        //if (webCameraTexture != null && webCameraTexture.didUpdateThisFrame) {
                        cameraTexture.Play();
                        //}

                        break;
                    }
                }
		    }
//            cameraTexture = new WebCamTexture();  

            print("CAM CON PRE PLAY");
            if(cameraTexture != null)
			    cameraTexture.Play();
			isPlay = true;  
		}
        print("CAM CON STARTED");

        
	}


	public void StopWork()
	{
		this.cameraTexture.Stop();
	}

}

