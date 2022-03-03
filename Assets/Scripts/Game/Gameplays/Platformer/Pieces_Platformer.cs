using System;
using System.Collections;
using System.Collections.Generic;
using Gameplays.Platformer;
using UnityEngine;

public class Pieces_Platformer : MonoBehaviour
{
    [SerializeField] private CountPieces_Platformer count;
    [SerializeField] private AudioManager audio;

    private void Start()
    {
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //dont heal if detected is not player
        if (other.tag.Equals("Player") == false)
        {
            return;
        }

        //check if player entity is not null
        if (other.gameObject.CompareTag("Player"))
        {
            audio.sounds[9].volume = 1;
            audio.Play("Pieza");
            count.NumPieces = count.NumPieces + 1;
            //print("Piezas recogidas: " + count.NumPieces);

            Destroy(this.gameObject);
        }
    }
}
