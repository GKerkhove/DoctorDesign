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

                    foreach (Person p in data)
                    {
                        print(p.FirstName + p.LastName);
                        NameList.Add("" + p.FirstName + " " + p.LastName);

                    }
                    foreach (string str in NameList)
                    {
                        GameObject go = Instantiate(Button_Template) as GameObject;
                        go.SetActive(true);
                        Tutorial_Button TB = go.GetComponent<Tutorial_Button>();
                        TB.SetName(str);
                        go.transform.SetParent(ToAddTo.transform);
                        go.transform.localScale = new Vector3(1, 1, 1);
                        go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 1);

                    }
                });
            }
        }
        else if (input.text.Length == 0)
        {
            Debug.Log("Main Input Empty");
        }
    }

    public void Start()
    {
        //Adds a listener that invokes the "LockInput" method when the player finishes editing the main input field.
        //Passes the main input field into the method when "LockInput" is invoked
        input.onEndEdit.AddListener(delegate { LockInput(input); });
    }
}
