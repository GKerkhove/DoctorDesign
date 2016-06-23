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

    private GameObject CreateListItem(Person p, bool check)
    {
        
        DatabaseManager.Get().RetrieveImage(p.Email, data =>
        {
            GameObject go = Instantiate(Button_Template) as GameObject;
            go.SetActive(true);
            Tutorial_Button TB = go.GetComponent<Tutorial_Button>();
            TB.SetName(p.FirstName + " " + p.LastName);
            go.transform.SetParent(ToAddTo.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(go.transform.localPosition.x,
            go.transform.localPosition.y, 1);
            print("pietje puk");
            Debug.Log("Ik heb image gevonden");
            print(data);

            if (data != null)
            {
                p.Picture = Sprite.Create(data, new Rect(0, 0, data.width, data.height), new Vector2(1, 1));
                p.Picture.name = p.Email + ".jpg";
            }
            else
            {
                p.Picture = Game.Get().StandardPerson;
            }
            go.transform.Find("Avatar").GetComponent<Image>().sprite = p.Picture;
            go.transform.Find("Avatar").GetComponent<Image>().color = new Color(255,255,255,255);
            if (check)
            {
                go.transform.Find("Icon").gameObject.SetActive(true);
            }
            go.GetComponent<Button>().onClick.AddListener(() => ShowPersonDetails(p));

        });
        return null;
    }

    void ShowPersonDetails(Person p)
    {
        print(p);
        Game.Get().MainDialog.ShowLarge(p.FirstName + " " + p.LastName,p.Email + "\n" + p.JobFunction + "\n"+p.CompanyName, "notes...", p.Picture,true);
    }
    void LockInput(InputField input)
    {
        string items = null;
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

                items = string.Join(",", list);
                print(items);
            }
            if (items != null) {
                url = "http://jimiverhoeven.nl:8080/search?user=DocterDesign&search=" + items;
            }
            else
            {
                url = "http://jimiverhoeven.nl:8080/search?user=DocterDesign&search=" + input.text;
            }
            Debug.Log(url);
            if (url != null)
            {
                print("NOT NULL");
                DatabaseManager.Get().SearchUser(url, (data) =>
                {
                    DatabaseManager.Get().RetrieveConnectedPersons(data2 =>
                    {
                        print("waarom");
                        foreach (Person p in data)
                        {
                            bool v = false;
                            foreach(Person p2 in data2){
                                if (p2.Email == p.Email)
                                {
                                    v = true;
                                    break;
                                }
                            }
                            GameObject go = CreateListItem(p, v);
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

            GameObject go = CreateListItem(p, true);
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
