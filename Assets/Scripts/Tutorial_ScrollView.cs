using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView : MonoBehaviour {

	public GameObject Button_Template;
	private List<string> NameList = new List<string>();

	// Use this for initialization
	void Start () {

       DatabaseManager.Get().retrieveAll(data =>
        {
            
            foreach(Person p in data)
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
                go.transform.SetParent(Button_Template.transform.parent);

            }
        });

		
	}

    public void ButtonClicked(string str)
	{
		Debug.Log(str + " button clicked.");

	}
}
