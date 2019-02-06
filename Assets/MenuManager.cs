using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Play");
    }

    public void ReturnToTheGame()
    {
        gameObject.SetActive(false);
        GameManager.isCanvasMenu = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void MenuReturn()
    {
        SceneManager.LoadScene(0);
    }
}
