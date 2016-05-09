using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour {

    public Button First;
    public Button Second;
    public Button Third;
    public GameObject Panel1;
    public GameObject Panel2;
    public GameObject Panel3;
    public GameObject Panel4;
    public GameObject ExitPanel;
	// Use this for initialization
	void Start () {
        First.onClick.AddListener(FirstClick);
        Second.onClick.AddListener(SecondClick);
        Third.onClick.AddListener(ThirdClick);
        ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
        ExitPanel.transform.SetParent(gameObject.transform);
        ExitPanel.transform.localPosition = new Vector3(0, 0, 0);
        ExitPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(ExitClick);
        ExitPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(CloseClick);
	    DatabaseManager.Get().retrieveAll(data =>
	    {
	        print(data[0].FirstName);
//	        Panel1.transform.Find("Name").GetComponent<Text>().text = data[0].FirstName;
	    });

	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            print("test");
//            if (ExitPanel == null)
//            {
//                ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
//                ExitPanel.transform.SetParent(gameObject.transform);
//                ExitPanel.transform.position = new Vector3(0,0,0);
//                ExitPanel.SetActive(true);
//                ExitPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(ExitClick);
//                ExitPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(CloseClick);
//            }
            ExitPanel.SetActive(!ExitPanel.activeSelf);
        }
    }

    void ExitClick()
    {
        Application.Quit();
    }

    void CloseClick()
    {
        ExitPanel.SetActive(true);
    }

    void FirstClick()
    {
        print("lol");
        Panel1.SetActive(false);
        Panel2.SetActive(true);
    }
    void SecondClick()
    {
        print("lol");
        Panel2.SetActive(false);
        Panel3.SetActive(true);
    }
    void ThirdClick()
    {
        print("lol");
        Panel3.SetActive(false);
        Panel4.SetActive(true);
    }
    
}
