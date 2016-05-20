using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TakeProfilePicture : MonoBehaviour {
    WebCamTexture _CamTex;
    private string _SavePath = "C:/WebcamSnaps/";
    int _CaptureCounter = 0;
    public Button startPhoto;
    public Button takePhoto;

    // Use this for initialization
    void Start()
    {
        startPhoto.onClick.AddListener(FirstClick);
        takePhoto.onClick.AddListener(TakeSnapshot);
    }

    

    void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(_CamTex.width, _CamTex.height);
        snap.SetPixels(_CamTex.GetPixels());
        snap.Apply();

        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        ++_CaptureCounter;
    }

    void FirstClick()
    {
        Game.Get().qrController.StartCamera();
    }
}
