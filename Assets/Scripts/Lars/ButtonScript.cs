using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public GameObject thisPage;
    public GameObject nextPage;

    public void Button()
    {
        thisPage.SetActive(false);
        nextPage.SetActive(true);
        if (nextPage.tag == "MainCanvas")
        {
            Game.Get().CurrentCanvas = nextPage;
        }
    }
}