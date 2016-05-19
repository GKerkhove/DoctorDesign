using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public GameObject thisPage;
    public GameObject nextPage;

    public void Button()
    {
        thisPage.active = false;
        nextPage.active = true;
    }
}