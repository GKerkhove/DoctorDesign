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
            print(data[0].FirstName);
            //	        Panel1.transform.Find("Name").GetComponent<Text>().text = data[0].FirstName;
        });
	    
        foreach
	    {
	        
	    }
		NameList.Add("Alan");
		NameList.Add("Amy");
		NameList.Add("Brian");
		NameList.Add("Carrie");
		NameList.Add("David");
		NameList.Add("Joe");
		NameList.Add("Jason");
		NameList.Add("Michelle");
		NameList.Add("Stephanie");
		NameList.Add("Zoe");

		foreach(string str in NameList)
		{
			GameObject go = Instantiate(Button_Template) as GameObject;
			go.SetActive(true);
			Tutorial_Button TB = go.GetComponent<Tutorial_Button>();
			TB.SetName(str);
			go.transform.SetParent(Button_Template.transform.parent);

		}


	}

    public void ButtonClicked(string str)
	{
		Debug.Log(str + " button clicked.");

	}
}
