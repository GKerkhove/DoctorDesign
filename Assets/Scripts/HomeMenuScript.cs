﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeMenuScript : MonoBehaviour {

    public Button StatusButton;
    public Button MapButton;
    public Button AgendaButton;

    public GameObject StatusPanel;
    public GameObject MapPanel;
    public GameObject AgendaPanel;

    public GameObject Selector;
    private Vector3 StatusSelector;
    private Vector3 MapSelector;
    private Vector3 AgendaSelector;

    // Use this for initialization
    void Start () {
        StatusButton.onClick.AddListener(StatusClicked);
        MapButton.onClick.AddListener(MapClicked);
        AgendaButton.onClick.AddListener(AgendaClicked);
        StatusSelector = new Vector3(-150f, -80.8f, 0);
        MapSelector = new Vector3(0f, -80.8f, 0);
        AgendaSelector = new Vector3(150f, -80.8f, 0);
    }
	
    void StatusClicked()
    {
        StatusPanel.SetActive(true);
        MapPanel.SetActive(false);
        AgendaPanel.SetActive(false);
        Selector.transform.localPosition = StatusSelector;
    }

    void MapClicked()
    {
        StatusPanel.SetActive(false);
        MapPanel.SetActive(true);
        AgendaPanel.SetActive(false);
        Selector.transform.localPosition = MapSelector;

    }
    void AgendaClicked()
    {
        StatusPanel.SetActive(false);
        MapPanel.SetActive(false);
        AgendaPanel.SetActive(true);
        Selector.transform.localPosition = AgendaSelector;

    }

}
