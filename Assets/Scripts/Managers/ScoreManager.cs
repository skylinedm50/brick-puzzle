
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour {


    int m_score = 0;
    int m_lines = 0;
    public int m_level = 1;

    public Text LinesText;
    public Text LevelText;
    public Text ScoreText;
    public bool isLevelUp = false;
    public bool isMainScreen;

    int linesCount = 0;

    public int linesPerlevel = 8;

    const int m_minLines = 1;
    const int m_maxLines = 4;


    // Use this for initialization
    void Start () {
        Reset();
        UpdateUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetCurrentScore() {
        return m_score;
    }
    
    public int GetBestScore() {
        return PlayerPrefs.GetInt("Score",0);
    } 

   
    public void SaveScore() {

        if (PlayerPrefs.GetInt("Score") <= m_score)
        {
            PlayerPrefs.SetInt("Score", m_score);
            PlayerPrefs.Save();
        }

    }

    void UpdateUI() {
        if (!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.Save();
        }

        if (!isMainScreen) {
            LinesText.text = linesCount.ToString();
            LevelText.text = m_level.ToString();
            ScoreText.text = ScoreZeros(m_score, 5);
        }
    }

    public void scoreLines(int n)
    {
        n = Mathf.Clamp(n, m_minLines, m_maxLines);
        isLevelUp = false;
        switch (n)
        {
            case 1: m_score += 40 * m_level;
                    break;
            case 2: m_score += 100 * m_level;
                break;
            case 3: m_score += 300 * m_level;
                break;
            case 4: m_score += 1200 * m_level;
                break;
        }

        m_lines -= n;
        linesCount += n;

        if (m_lines <= 0) {           
            LevelUp();
        }
        UpdateUI();
    }

    public void Reset() {
        m_level = 1;
        m_lines = linesPerlevel * m_level;
        UpdateUI();
    }

    string ScoreZeros(int n , int zeros) {
        string value = n.ToString();
        while (value.Length < zeros) {
            value = "0" + value;
        }
        return value;
    }

    public void LevelUp() {
        m_level++;
        isLevelUp = true;
        m_lines = linesPerlevel * m_level;
    }
   
}
