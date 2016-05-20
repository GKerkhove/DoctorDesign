﻿/// <summary>
/// write by 52cwalk,if you have some question ,please contract lycwalk@gmail.com
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QRDecodeTest : MonoBehaviour {

	public QRCodeDecodeController e_qrController;

	public Text UiText;
    public GameObject startBtn;
    public GameObject startBtn2;
	public GameObject scanLineObj;

//    public GameObject MainCanvas;

	// Use this for initialization
	void Start () {
		if (e_qrController != null) {
			e_qrController.e_QRScanFinished += qrScanFinished;
		}
        startBtn.GetComponent<Button>().onClick.AddListener(StartScan);
        startBtn2.GetComponent<Button>().onClick.AddListener(StartScan);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void qrScanFinished(string dataText)
	{
        print(dataText);
        Game.Get().CurrentCanvas.SetActive(true);
        e_qrController.StopCamera();
	    if (Game.Get().CurrentCanvas.tag == "MainCanvas")
	    {
	        UiText.text = dataText;
	    }
	}

	/// <summary>
	/// reset the QRScanner Controller 
	/// </summary>
	public void StartScan()
	{

		if (e_qrController != null) {
			e_qrController.Reset();
		}
        e_qrController.StartCamera();

//		if (UiText != null) {
//			UiText.text = "";	
//		}
		
//		if (resetBtn != null) {
//			resetBtn.SetActive(false);
//		}
        if(Game.Get().CurrentCanvas.name != "StartCanvas")
            Game.Get().CurrentCanvas.SetActive(false);
	}
	/// <summary>
	/// if you want to go to other scene ,you must call the QRCodeDecodeController.StopWork(),otherwise,the application will crashed on Mobile .
	/// </summary>
	/// <param name="scenename">Scenename.</param>
	public void GotoNextScene(string scenename)
	{
		if (e_qrController != null) {
			e_qrController.StopWork();
		}
		Application.LoadLevel (scenename);
	}

}
