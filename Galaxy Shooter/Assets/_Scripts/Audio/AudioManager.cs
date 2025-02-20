using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource[] audioSFX;
    public AudioSource levelMusic, bossMusic, victoryMusic, loseMusic;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        levelMusic.Play();

    }

    void StopAllMusic()
    {
        levelMusic.Stop();
        bossMusic.Stop();
        loseMusic.Stop();
        victoryMusic.Stop();

    }
    public void LevelMusic()
    {
        StopAllMusic();
        levelMusic.Play();
    }

   

    public void VictoryMusic()
    {
        StopAllMusic();
        victoryMusic.Play();
    }

    public void LoseMusic()
    {
        StopAllMusic();
        loseMusic.Play();
    }

    public void PlaySFX(int sfx)
    {
        audioSFX[sfx].Stop();
        audioSFX[sfx].Play();
    }
}
