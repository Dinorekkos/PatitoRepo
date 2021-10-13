using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

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
            playerListener.enabled = false;
        }
        else if (pausePanel.activeInHierarchy == false)
        {
            playerListener.enabled = true;
        }
    }
}
