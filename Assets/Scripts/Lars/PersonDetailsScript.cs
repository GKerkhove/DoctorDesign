using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PersonDetailsScript : MonoBehaviour
{

    public Button gegevensButton;
    public Button aantekeningenButton;

    public GameObject gegevensPanel;
    public GameObject aantekeningenPanel;

    public GameObject Selector;

    private Vector3 gegevensSelector;
    private Vector3 aantekeningenSelector;

    // Use this for initialization
    void Start()
    {
        //        Person p = new Person();
        //        p.FirstName = "Test naam";
        //        CurrentUser.AddPerson(p);
        //        p = CurrentUser.GetPerson();
        //        print(p.FirstName);
        gegevensButton.onClick.AddListener(GegevensClicked);
        aantekeningenButton.onClick.AddListener(AantekeningenClicked);
        gegevensSelector = new Vector3(-150f, -80.8f, 0);
        aantekeningenSelector = new Vector3(150f, -80.8f, 0);
    }

    void GegevensClicked()
    {
        gegevensPanel.SetActive(true);
        aantekeningenPanel.SetActive(false);
        Selector.transform.localPosition = gegevensSelector;
    }

    void AantekeningenClicked()
    {
        aantekeningenPanel.SetActive(true);
        gegevensPanel.SetActive(false);
        Selector.transform.localPosition = aantekeningenSelector;

    }
}
