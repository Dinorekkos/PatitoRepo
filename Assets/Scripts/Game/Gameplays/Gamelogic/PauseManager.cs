using System.Collections;
using System.Collections.Generic;
using DialogSystem;
using Gameplays.Platformer;
using UnityEngine;

public class PauseManager : MonoBehaviour

{    
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject[] audiosGO;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioListener playerListener;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pausePanel.activeInHierarchy == true)
        {
           CallMutEPause();
        }

        if (pausePanel.activeInHierarchy == false)
        {
            audioManager.CallChangesAudioManager();
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].mute = false;
            }
            for (int i = 0; i < audiosGO.Length; i++)
            {
                audiosGO[i].GetComponent<AudioSource>().mute = false;
            }
            audioManager.CallChangesAudioManager();
        }
    }

    public void CallMutEPause()
    {
        audioManager.CallChangesAudioManager();
        for (int i = 0; i < audioManager.sounds.Length; i++)
        {
            audioManager.sounds[i].mute = true;
        }
        for (int i = 0; i < audiosGO.Length; i++)
        {
            audiosGO[i].GetComponent<AudioSource>().mute = true;
        }
        audioManager.CallChangesAudioManager();
    }
    
        
        
}

