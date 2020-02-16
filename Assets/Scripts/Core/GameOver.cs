using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text BestScore;
    public Text CurrentScore;
    ScoreManager m_ScoreManager;
    public GameObject GameOverLoadPanel;
    AdsScript ads;

    // Use this for initialization
    void Start () {
        this.m_ScoreManager = GameObject.FindObjectOfType<ScoreManager>();
        this.ads = GameObject.FindObjectOfType<AdsScript>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void HideGameOverPanel() {
        this.GameOverLoadPanel.SetActive(false);
    }


    string ScoreZeros(int n, int zeros)
    {
        string value = n.ToString();
        while (value.Length < zeros)
        {
            value = "0" + value;
        }
        return value;
    }

    public void ShowPanelGameOver(int CurrentScore) {
       
       // this.ads.UnityVideoAds();
        GameOverLoadPanel.SetActive(true);
        this.BestScore.text = ScoreZeros(m_ScoreManager.GetBestScore(), 5);
        this.CurrentScore.text = ScoreZeros(CurrentScore, 5);
      
    }
}
