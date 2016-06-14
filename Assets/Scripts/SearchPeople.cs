using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SearchPeople : MonoBehaviour {
    public InputField input;
    string[] list = null;
    string url = null;
    public GameObject Button_Template;
    private List<string> NameList = new List<string>();
    public GameObject ToAddTo;

    private GameObject CreateListItem(Person p)
    {
        GameObject go = Instantiate(Button_Template) as GameObject;
        go.SetActive(true);
        Tutorial_Button TB = go.GetComponent<Tutorial_Button>();
        TB.SetName(p.FirstName + " " + p.LastName);
        go.transform.SetParent(ToAddTo.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localPosition = new Vector3(go.transform.localPosition.x,
        go.transform.localPosition.y, 1);
        go.GetComponent<Button>().onClick.AddListener(() => ShowPersonDetails(p));
        return go;
    }

    void ShowPersonDetails(Person p)
    {
        print(p);
        Game.Get().MainDialog.ShowLarge(p.FirstName + " " + p.LastName,p.Email + "\n" + p.JobFunction + "\n"+p.CompanyName, "notes...", p.Picture);
    }
    void LockInput(InputField input)
    {
        print("test");
        foreach (Transform child in ToAddTo.transform)
        {
            Destroy(child.gameObject);
            NameList.Clear();
        }
        if (input.text.Length > 0)
        {
            input.text = input.text.ToLower();
            if(input.text.Contains(" "))
            {
                list = input.text.Split(null);
            }
            if (list != null) {
                url = "http://jimiverhoeven.nl:8080/search?user=DocterDesign&search=" + list;
            }
            else
            {
                url = "http://jimiverhoeven.nl:8080/search?user=DocterDesign&search=" + input.text;
            }
            Debug.Log(url);
            if (url != null)
            {
                DatabaseManager.Get().SearchUser(url, (data) =>
                {
                    DatabaseManager.Get().RetrieveConnectedPersons(data2 =>
                    {
                        foreach (Person p in data)
                        {
                            GameObject go = CreateListItem(p);
                            foreach(Person p2 in data2){
                                if (p2.Email == p.Email)
                                {
                                    go.transform.Find("Icon").gameObject.SetActive(true);
                                    break;
                                }
                            }
                        }
                    });
                });
            }
        }
        else if (input.text.Length == 0)
        {
            Debug.Log("Main Input Empty");
            DatabaseManager.Get().RetrieveConnectedPersons(data =>
            {
                Game.Get().SearchScript.CreatePeopleList(data);
            });
        }
    }

    public void CreatePeopleList(List<Person> people)
    {
        print("CREATE PEOPLE LIST   ");
        foreach (Person p in people)
        {

            GameObject go = CreateListItem(p);
            go.transform.Find("Icon").gameObject.SetActive(true);
        }
    }

    public void Start()
    {
        //Adds a listener that invokes the "LockInput" method when the player finishes editing the main input field.
        //Passes the main input field into the method when "LockInput" is invoked
        input.onEndEdit.AddListener(delegate { LockInput(input); });
        Game.Get().SearchScript = this;
    }
}
