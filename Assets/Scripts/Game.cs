using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public GameObject ExitPanel;
    public GameObject CurrentCanvas;
    public GameObject HomeCanvas;
    private static Game _instance;
    public QRCodeDecodeController qrController;
    public Person User;
    public readonly bool DEBUG = false;
    //public UnityEvent _userScanned;
    public delegate void UserScanned(Person p);
    public event UserScanned userScanned;  

    private bool CameraShown = false;

    public static Game Get()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start ()
	{

        //if (_userScanned == null)
        //    _userScanned = new UnityEvent();


        print(Application.persistentDataPath);
	    if (CurrentUser.HasPerson())
	    {
	        if (!DEBUG)
	        {
	            CurrentCanvas.SetActive(false);
	            CurrentCanvas = HomeCanvas;
	            CurrentCanvas.SetActive(true);
	            User = CurrentUser.GetPerson();
	        }
	    }
        DontDestroyOnLoad(gameObject);
        ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
        GameObject go = GameObject.FindGameObjectWithTag("MainCanvas");
	    if (go == null)
	    {
            go = GameObject.Find("Canvas");
	    }
        ExitPanel.transform.SetParent(go.transform);
        ExitPanel.transform.localPosition = new Vector3(0, 0, 0);
        ExitPanel.transform.localScale = new Vector3(1,1,1);
        ExitPanel.transform.Find("Exit").Find("Button Layer").GetComponent<Button>().onClick.AddListener(ExitClick);
        ExitPanel.transform.Find("Close").Find("Button Layer").GetComponent<Button>().onClick.AddListener(CloseClick);
        //ExitPanel.SetActive(false);
	}

    void ExitClick()
    {
        Application.Quit();
    }

    void CloseClick()
    {
        ExitPanel.SetActive(false);
        if(CameraShown)
            qrController.StartCamera();
        CameraShown = false;
    }

    public void TriggerScanned(Person p)
    {
        userScanned(p);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ExitPanel == null)
            {
                ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
                ExitPanel.transform.SetParent(gameObject.transform);
                ExitPanel.transform.position = new Vector3(0, 0, 0);
                ExitPanel.SetActive(true);
                ExitPanel.transform.Find("Exit").Find("Button Layer").GetComponent<Button>().onClick.AddListener(ExitClick);
                ExitPanel.transform.Find("Close").Find("Button Layer").GetComponent<Button>().onClick.AddListener(CloseClick);
            }
            ExitPanel.transform.Find("Exit").Find("Button Layer").GetComponent<Button>().onClick.AddListener(ExitClick);
            ExitPanel.transform.Find("Close").Find("Button Layer").GetComponent<Button>().onClick.AddListener(CloseClick);
            ExitPanel.SetActive(!ExitPanel.activeSelf);
            if (qrController.e_DeviceController.cameraTexture.isPlaying)
            {
                qrController.StopCamera();
                CameraShown = true;
            }
        }
	}
}
