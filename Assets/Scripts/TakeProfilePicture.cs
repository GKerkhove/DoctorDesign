using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TakeProfilePicture : MonoBehaviour {
    WebCamTexture _CamTex;
    public Button startPhoto;
    public Button takePhoto;
    public Image profilePicture;
    private Texture2D snap;
    // Use this for initialization
    void Start()
    {
        startPhoto.onClick.AddListener(FirstClick);
        takePhoto.onClick.AddListener(TakeSnapshot);
    }

    

    void TakeSnapshot()
    {
        _CamTex = Game.Get().qrController.e_DeviceController.cameraTexture;
        snap = new Texture2D(_CamTex.width, _CamTex.height);
        snap.SetPixels(_CamTex.GetPixels());
        snap.Apply();
        if (snap != null)
        {
			string email = Game.Get().User.Email;
            Debug.Log(snap);
            DatabaseManager.Get().uploadImage(snap, email);
        }
        takePhoto.gameObject.SetActive(false);
        Game.Get().qrController.StopCamera();
        profilePicture.gameObject.SetActive(true);
        print(snap);
        profilePicture.sprite = Sprite.Create(snap, new Rect(0, 0, snap.width, snap.height), new Vector2(0.5f, 0.5f));


    }


    void FirstClick()
    {
        Game.Get().qrController.StartCamera(true);
        Game.Get().qrController.transform.rotation = new Quaternion(Game.Get().qrController.transform.rotation.x, Game.Get().qrController.transform.rotation.y, Game.Get().qrController.transform.rotation.z+90,0);
        takePhoto.gameObject.SetActive(true);
    }
}
