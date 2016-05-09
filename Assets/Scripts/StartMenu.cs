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
	// Use this for initialization
	void Start () {
        First.onClick.AddListener(FirstClick);
        Second.onClick.AddListener(SecondClick);
        Third.onClick.AddListener(ThirdClick);
	    DatabaseManager.Get().retrieveAll(data =>
	    {
	        print(data[0].FirstName);
//	        Panel1.transform.Find("Name").GetComponent<Text>().text = data[0].FirstName;
	    });
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
