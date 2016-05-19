﻿/// <summary>
/// write by 52cwalk,if you have some question ,please contract lycwalk@gmail.com
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QRDecodeTest : MonoBehaviour {

	public QRCodeDecodeController e_qrController;

//	public Text UiText;
	public GameObject StartQrBtn;
	public GameObject scanLineObj;
    public GameObject userCamera;
//    public GameObject normalCamera;
    public GameObject backgroundPanel;
    public GameObject mainPanel;

	// Use this for initialization
	void Start () {
		if (e_qrController != null) {
            e_qrController.e_QRScanFinished += qrScanFinished;
//            e_qrController.StopWork();

		}
        StartQrBtn.GetComponent<Button>().onClick.AddListener(StartQR);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void qrScanFinished(string dataText)
	{
	    print("Finished: " + dataText);
//		UiText.text = dataText;
		if (StartQrBtn != null) {
			StartQrBtn.SetActive(true);
		}
//        normalCamera.SetActive(true);
        userCamera.SetActive(false);
        mainPanel.SetActive(true);
        backgroundPanel.SetActive(false);
        scanLineObj.SetActive(false);
		if(scanLineObj != null)
		{
			scanLineObj.SetActive(false);
		}
	}

    /// <summary>
    /// reset the QRScanner Controller 
    /// </summary>
    public void StartQR()
    {
//        e_qrController.StopWork();
//        normalCamera.SetActive(false);
        userCamera.SetActive(true);
//        userCamera.transform.Find("CameraPlane").gameObject.SetActive(true);
        StartQrBtn.SetActive(false);
        backgroundPanel.SetActive(true);
        mainPanel.SetActive(false);
        scanLineObj.SetActive(true);


        e_qrController.Reset();

    }

    /// <summary>
	/// if you want to go to other scene ,you must call the QRCodeDecodeController.StopWork(),otherwise,the application will crashed on Mobile .
	/// </summary>
//	/// <param name="scenename">Scenename.</param>
	public void GotoNextScene(string scenename)
	{
		if (e_qrController != null) {
			e_qrController.StopWork();
		}
		Application.LoadLevel (scenename);
	}

}
