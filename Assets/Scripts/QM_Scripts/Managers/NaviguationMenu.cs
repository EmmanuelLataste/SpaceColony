﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NaviguationMenu : MonoBehaviour {

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject selectedObject;
    [SerializeField] bool buttonSelected;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }

    public void SelectedObject(GameObject newSO)
    {
        eventSystem.SetSelectedGameObject(newSO);
    }
}
