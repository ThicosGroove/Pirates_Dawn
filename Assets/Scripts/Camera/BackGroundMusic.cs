using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using GameEvents;

[DefaultExecutionOrder(10)]
public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    private AudioSource audioSource;

    private void OnEnable()
    {
        GameplayEvents.MainMenu += PlayMusic;
        GameplayEvents.GameStart += PlayMusic;
    }

    private void OnDisable()
    {
        GameplayEvents.MainMenu -= PlayMusic;
        GameplayEvents.GameStart -= PlayMusic;      
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    void PlayMusic()
    {
        if (GameplayManager.Instance.currentGameState == GameStates.MAIN_MENU
          || GameplayManager.Instance.currentGameState == GameStates.PREPLAY)
        {
            audioSource.clip = clips[Const.INITIAL_BG_MUSIC];
            audioSource.Play();
        }
        else if (GameplayManager.Instance.currentGameState == GameStates.GAMESTART
                || GameplayManager.Instance.currentGameState == GameStates.PLAYING)
        {
            audioSource.clip = clips[Const.PLAY_BG_MUSIC];
            audioSource.Play();
        }
    }


}
