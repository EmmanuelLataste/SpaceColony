using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Use this for initialization

    [SerializeField] GameObject canvasMenu;
    public static bool isCanvasMenu;
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        Pause();
	}

    void Pause()
    {
        if (Input.GetButtonDown("Escape") && isCanvasMenu == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canvasMenu.SetActive(true);
            isCanvasMenu = true;
            Time.timeScale = 0;
        }

        else if (Input.GetButtonDown("Escape") && isCanvasMenu == true)
        {
            canvasMenu.SetActive(false);
            isCanvasMenu = false;
            Time.timeScale = 1;
        }


    }
}
