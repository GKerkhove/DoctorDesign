using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeMenuScript : MonoBehaviour {

    public Button StatusButton;
    public Button MapButton;
    public Button AgendaButton;

    public GameObject StatusPanel;
    public GameObject MapPanel;
    public GameObject AgendaPanel;

    // Use this for initialization
    void Start () {
        StatusButton.onClick.AddListener(StatusClicked);
        MapButton.onClick.AddListener(MapClicked);
        AgendaButton.onClick.AddListener(AgendaClicked);
    }
	
	// Update is called once per frame
	void Update () {
	
	}



    void StatusClicked()
    {
        print("lol");
        StatusPanel.SetActive(true);
        MapPanel.SetActive(false);
        AgendaPanel.SetActive(false);
    }

    void MapClicked()
    {
        print("lol");
        StatusPanel.SetActive(false);
        MapPanel.SetActive(true);
        AgendaPanel.SetActive(false);
    }
    void AgendaClicked()
    {
        print("lol");
        StatusPanel.SetActive(false);
        MapPanel.SetActive(false);
        AgendaPanel.SetActive(true);
    }

}
