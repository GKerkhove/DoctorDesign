using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour {

    public Button First;
    public GameObject Panel1;
    public GameObject Panel2;
	// Use this for initialization
	void Start () {
        First.onClick.AddListener(FirstClick);
	}

    void FirstClick()
    {
        print("lol");
        Panel1.SetActive(false);
        Panel2.SetActive(true);
    }
    
}
