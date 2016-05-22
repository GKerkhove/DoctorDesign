using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SearchPeople : MonoBehaviour {
    public InputField input;
    string[] list = null;
    string url = null;
    void LockInput(InputField input)
    {
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
                    Debug.Log(data[0]);
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
