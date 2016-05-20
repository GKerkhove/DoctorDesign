using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TakeProfilePicture : MonoBehaviour {
    WebCamTexture _CamTex;
    public Button startPhoto;
    public Button takePhoto;
    public Image profilePicture;
    // Use this for initialization
    void Start()
    {
        startPhoto.onClick.AddListener(FirstClick);
        takePhoto.onClick.AddListener(TakeSnapshot);
    }

    

    void TakeSnapshot()
    {
        _CamTex = Game.Get().qrController.e_DeviceController.cameraTexture;
        Texture2D snap = new Texture2D(_CamTex.width, _CamTex.height);
        snap.SetPixels(_CamTex.GetPixels());
        snap.Apply();
        //WWWForm form = new WWWForm();
        //form.AddField("frameCount", Time.frameCount.ToString());
        //form.AddBinaryData("fileUpload", snap.GetRawTextureData(), "screenShot.png", "image/png");
        //WWW w = new WWW("http://jimiverhoeven.nl:8080/uploadImage", form);
        //yield return w;

        takePhoto.gameObject.SetActive(false);
        Game.Get().qrController.StopCamera();
        profilePicture.gameObject.SetActive(true);
        profilePicture.sprite = Sprite.Create(snap, new Rect(0, 0, snap.width, snap.height), new Vector2(0.5f, 0.5f));


    }

    void FirstClick()
    {
        Game.Get().qrController.StartCamera();
        takePhoto.gameObject.SetActive(true);
    }
}
