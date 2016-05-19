using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public GameObject ExitPanel;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
        GameObject go = GameObject.FindGameObjectWithTag("MainCanvas");
	    if (go == null)
	    {
            go = GameObject.Find("Canvas");
	    }
        ExitPanel.transform.SetParent(go.transform);
        ExitPanel.transform.localPosition = new Vector3(0, 0, 0);
        ExitPanel.transform.localScale = new Vector3(1,1,1);
        ExitPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(ExitClick);
        ExitPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(CloseClick);
	}

    void ExitClick()
    {
        Application.Quit();
    }

    void CloseClick()
    {
        ExitPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ExitPanel == null)
            {
                ExitPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Exit Panel"));
                ExitPanel.transform.SetParent(gameObject.transform);
                ExitPanel.transform.position = new Vector3(0, 0, 0);
                ExitPanel.SetActive(true);
                ExitPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(ExitClick);
                ExitPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(CloseClick);
            }
            ExitPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(ExitClick);
            ExitPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(CloseClick);
            ExitPanel.SetActive(!ExitPanel.activeSelf);
        }
	}
}
