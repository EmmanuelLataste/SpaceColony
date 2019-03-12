using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    // Use this for initialization

    [SerializeField] GameObject canvasMenu;
    [SerializeField] GameObject panelDeath;
    [SerializeField] string[] controller;
    [SerializeField] GameObject normalCam;
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] GameObject player;
    [SerializeField] CharacterController playerController;
    Life lifePlayer;
    public static bool isCanvasMenu;
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
       
        lifePlayer = player.GetComponent<Life>();
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update() {
        Save();
        Load();

        if (lifePlayer.isAlive == true)
        {
            Pause();
        }

        else StartCoroutine(Dead());

        DisableMouse();
      
        controller = Input.GetJoystickNames();
        normalCam.GetComponent<CameraController>().speedMouseX = MenuManager.sensivity;
        normalCam.GetComponent<CameraController>().speedMouseY = MenuManager.sensivity;


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

     IEnumerator Dead()
    {
        yield return new WaitForSeconds(2.5f);
        panelDeath.SetActive(true);
    }

    public void Save()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveSystem.SaveData(playerController);
        }

    }

    public void Load()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DataSave data = SaveSystem.LoadData();

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            player.transform.position = position;
        }
        
    }
}
