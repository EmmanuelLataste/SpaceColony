using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static float sensivity = 1;
    [SerializeField] Slider sliderSensivity;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        SceneManager.LoadScene(0);
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

    public void Disable(GameObject disableGO)
    {
        
            disableGO.SetActive(false);
        
    }

    public void Enable(GameObject enableGO)
    {
            enableGO.SetActive(true);
    }

    public void SensivityChange()
    {
        sensivity = sliderSensivity.value;
    }

   
}
