using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("GameObject")] 
    [SerializeField] private GameObject message;
    
    [SerializeField] private CountPieces_Platformer count;
    [SerializeField] private ChangeScene changeScene;
    [SerializeField] private int Scene;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (count.NumPieces == 8)
            {
                changeScene.FadeOut(Scene);
            }
            else if (count.NumPieces < 8)
            {
                message.SetActive(true);
            }
            
            
        }
    }
}
