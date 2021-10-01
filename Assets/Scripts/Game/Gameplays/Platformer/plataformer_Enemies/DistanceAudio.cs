using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAudio : MonoBehaviour
{
    private GameObject player;
    private AudioManager audioManager;

    [Header("Audio Clip")] 
    [SerializeField] private string clipName;

    //[SerializeField] private int clipPlaceArray;
    
    [Header("Distance Audio Interaction")]
    [SerializeField] private bool debugDistance;
    [SerializeField] bool canPlaySound;
    [SerializeField] private float audioDistance;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        canPlaySound = false;


    }

    private void Awake()
    {
        //audioManager.sounds[clipPlaceArray].volume = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MeasureDistance();
    }

    void PlaySound()
    {
        audioManager.Play(clipName);
    }

   void MeasureDistance()
    {
        float playerX, playerY, playerZ;
        float thisX,thisY,thisZ;
        float interactiveDistance;

        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        playerZ = player.transform.position.z;

        thisX = transform.position.x;
        thisY = transform.position.y;
        thisZ = transform.position.z;

        interactiveDistance = Mathf.Sqrt(Mathf.Pow((thisX - playerX),2) + Mathf.Pow((thisY - playerY),2) + Mathf.Pow((thisZ - playerZ),2));
        
        if(debugDistance) Debug.Log(interactiveDistance);

        if (interactiveDistance > audioDistance)
        {
            canPlaySound = false;
            PlaySound();
            //audioManager.sounds[clipPlaceArray].volume = 1;
            
        }
        if (interactiveDistance < audioDistance)
        {
            canPlaySound = true;
            //if(canPlaySound)
            
            
            
        }

        
    }
}
