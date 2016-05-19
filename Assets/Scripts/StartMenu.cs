using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour {

    public Button First;
    public Button Second;
    public Button Third;
    public Button Fourth;
    public GameObject Panel1;
    public GameObject Panel2;
    public GameObject Panel3;
    public GameObject Panel4;
	// Use this for initialization
	void Start () {
        First.onClick.AddListener(FirstClick);
        Second.onClick.AddListener(SecondClick);
        Third.onClick.AddListener(ThirdClick);
        Fourth.onClick.AddListener(FourthClick);
        
	    DatabaseManager.Get().retrieveAll(data =>
	    {
	        print(data[0].FirstName);
//	        Panel1.transform.Find("Name").GetComponent<Text>().text = data[0].FirstName;
	    });

	}

    void Update()
    {
//        if(Input.GetKeyDown(KeyCode.Escape))
//        {
//            print("test");
//            if (ExitPanel == null)
//            {
//                ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
//                ExitPanel.transform.SetParent(gameObject.transform);
//                ExitPanel.transform.position = new Vector3(0,0,0);
//                ExitPanel.SetActive(true);
//                ExitPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(ExitClick);
//                ExitPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(CloseClick);
//            }
//            ExitPanel.SetActive(!ExitPanel.activeSelf);
//        }
    }

    

    void FirstClick()
    {
        Panel1.SetActive(false);
        Panel2.SetActive(true);
    }
    void SecondClick()
    {
        Panel2.SetActive(false);
        Panel3.SetActive(true);
    }
    void ThirdClick()
    {
        Panel3.SetActive(false);
        Panel4.SetActive(true);
    }

    void FourthClick()
    {
        SceneManager.LoadScene("Home");
    }
    
}
