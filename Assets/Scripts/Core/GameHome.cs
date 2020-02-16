using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHome : MonoBehaviour {

    AdsScript Ads;

	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        this.Ads = GameObject.FindObjectOfType<AdsScript>();
        this.Ads.ShowBanner();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showHomePanel() {
        Time.timeScale = 1;        
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void startGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void CloseGame() {
        Application.Quit();
    }

}
