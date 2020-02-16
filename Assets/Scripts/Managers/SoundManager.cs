using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public bool m_FxEnabled = true;
    public bool m_MusicEnabled = true;

    [Range(0, 1)]
    public float m_MusicVolume = 1.0f;
    [Range(0,1)]
    public float m_FxVolume = 1.0f;

    public AudioClip m_ClearRowsSound;
    public AudioClip m_MoveSound;
    public AudioClip m_DropSound;
    public AudioClip m_GameOverSound;
    public AudioClip m_BackGroundMusic;
    public AudioSource m_audioSource;
    public AudioClip m_LevelUp;
    public AudioClip m_HoldSound;


    // Use this for initialization
    void Start () {
        playBackgroundMusic(m_BackGroundMusic);

    }
	
	// Update is called once per frame
	void Update () {
        UpdateMusic();
        
    }

    public void StopBackgroundMusic() {
        m_MusicEnabled = false;
        m_audioSource.Stop();
    }

    public void pauseMusic(bool state) {
        if (state) {
            m_audioSource.Pause();
            m_MusicEnabled = false;            
        } else {
            m_audioSource.UnPause();
            m_MusicEnabled = true;
        }
    }

    public void playBackgroundMusic(AudioClip musicClip) {
        if (!m_MusicEnabled || !musicClip || !m_audioSource) {
            return;
        }
        m_audioSource.Stop();
        m_audioSource.clip = musicClip;
        m_audioSource.volume = m_MusicVolume;
        m_audioSource.loop = true;
        m_audioSource.Play();
    }

    void UpdateMusic() {
        if (m_audioSource.isPlaying != m_MusicEnabled) {
            if (m_MusicEnabled){
                playBackgroundMusic(m_BackGroundMusic);
            }
            else {
                m_audioSource.Stop();
            }
        }
    }

    void ToggleMusic()
    {
        m_MusicEnabled = !m_MusicEnabled;
        UpdateMusic();
    }

}
