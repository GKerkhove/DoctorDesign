using System.Collections.Generic;
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

    public GameObject Selector;
    private Vector3 StatusSelector;
    private Vector3 MapSelector;
    private Vector3 AgendaSelector;
    public List<Sprite> Bobs;
    public Image Bob;

    // Use this for initialization
    void Start () {
//        Person p = new Person();
//        p.FirstName = "Test naam";
//        CurrentUser.AddPerson(p);
//        p = CurrentUser.GetPerson();
//        print(p.FirstName);
        StatusButton.onClick.AddListener(StatusClicked);
        MapButton.onClick.AddListener(MapClicked);
        AgendaButton.onClick.AddListener(AgendaClicked);
        StatusSelector = new Vector3(-150f, -80.8f, 0);
        MapSelector = new Vector3(0f, -80.8f, 0);
        AgendaSelector = new Vector3(150f, -80.8f, 0);
        Game.Get().userScanned += UserScanned;
    }

    void UserScanned(Person p)
    {
//        print(Game.Get().User.FirstName + " " + Game.Get().User.LastName);
        DatabaseManager.Get().retrieveConnections(Game.Get().User.Email, data =>
        {
            bool b = true;
            foreach (string s in data)
            {
                print(p.Email + " == " + s);
                if (p.Email != s)
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                Game.Get().BobState++;
                DatabaseManager.Get().CreateConnection(Game.Get().User.Email, p.Email, "");
                Game.Get()
                    .MainDialog.Show("Connected!", p.FirstName + " " + p.LastName,
                        "U bent succesvol verbonden.\n" + Game.Get().GetBobState(), p.Picture);
                if (Game.Get().BobState <= 2)
                {
                    Bob.sprite = Bobs[Game.Get().BobState];
                }
            }
            else
            {
                Game.Get().MainDialog.Show("Al Connected", "", "U bent al geconnect met " + p.FirstName + " " +p.LastName,null);
            }
        });
    }
	
    void StatusClicked()
    {
        StatusPanel.SetActive(true);
        MapPanel.SetActive(false);
        AgendaPanel.SetActive(false);
        Selector.transform.localPosition = StatusSelector;
    }

    void MapClicked()
    {
        StatusPanel.SetActive(false);
        MapPanel.SetActive(true);
        AgendaPanel.SetActive(false);
        Selector.transform.localPosition = MapSelector;

    }
    void AgendaClicked()
    {
        StatusPanel.SetActive(false);
        MapPanel.SetActive(false);
        AgendaPanel.SetActive(true);
        Selector.transform.localPosition = AgendaSelector;

    }

}
