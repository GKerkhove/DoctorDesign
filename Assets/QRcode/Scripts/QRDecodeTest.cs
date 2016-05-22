/// <summary>
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

    public GameObject NextButtonLogin;
    public GameObject RetryQRButton;

//    public GameObject MainCanvas;

	// Use this for initialization
	void Start () {
		if (e_qrController != null) {
			e_qrController.e_QRScanFinished += qrScanFinished;
		    Game.Get().qrController = e_qrController;
		    if (Game.Get().DEBUG)
		    {
		        NextButtonLogin.SetActive(true);
                DatabaseManager.Get().retrieveByEmail("jann@info.nl",data =>
                {
                    Game.Get().User = data;

                    //	        Panel1.transform.Find("Name").GetComponent<Text>().text = data[0].FirstName;
                });
		    }
		}
        startBtn.GetComponent<Button>().onClick.AddListener(StartScan);
        startBtn2.GetComponent<Button>().onClick.AddListener(StartScan);
        RetryQRButton.GetComponent<Button>().onClick.AddListener(StartScan);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void qrScanFinished(string dataText)
	{
        print(dataText);
//        Game.Get().CurrentCanvas.SetActive(true);
	    if (Game.Get().CurrentCanvas.name == "StartCanvas")
	    {
//	        UiText.text = dataText;
            DatabaseManager.Get().retrieveByEmail(dataText,data =>
            {
                if (data != null)
                {
                    RetryQRButton.SetActive(false);
                    e_qrController.StopCamera();
                    NextButtonLogin.SetActive(true);
                    Game.Get().User = data;
                    UiText.text = "U bent ingelogd als " + data.FirstName + " " + data.LastName + " op het email " +
                                  data.Email;
                    print("U bent ingelogd als " + data.FirstName + " " + data.LastName + " op het email " +
                          data.Email);
                }
                else
                {
                    print("bla123");
                    UiText.text = "QR code incorrect.";
                    RetryQRButton.SetActive(true);
                    e_qrController.StopCamera();

//                    e_qrController.StartCamera();
                }
                //	        Panel1.transform.Find("Name").GetComponent<Text>().text = data[0].FirstName;
            });
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
