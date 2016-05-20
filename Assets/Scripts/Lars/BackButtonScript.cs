using UnityEngine;
using System.Collections;

public class BackButtonScript : MonoBehaviour {

    public GameObject thisPage;
    public GameObject lastPage;

    public void Back()
    {
        thisPage.active = false;
        lastPage.active = true;
    }
}