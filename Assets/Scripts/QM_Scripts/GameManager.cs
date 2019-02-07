using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    // Use this for initialization

    [SerializeField] GameObject canvasMenu;
    [SerializeField] string[] controller;
    public static bool isCanvasMenu;
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        Pause();
        DisableMouse();
        controller = Input.GetJoystickNames();
     

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

    void DisableMouse()
    {
        if (controller.Length <= 0 || controller[0] == "")
        {
            CameraController.isControllerConnected = false;
        }

        else
        {
            CameraController.isControllerConnected = true;
        }

    }
}
