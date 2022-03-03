using System;
using System.Collections;
using System.Collections.Generic;
using Gameplays.Platformer;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;


namespace DialogSystem
{
    public class DialogManager : MonoBehaviour
    {
        [Header("Audio Manager")] 
        [SerializeField] private AudioManager audioManager;

        [SerializeField] private int placeInAudioManager;
        public Image finalImage;
        public CharacterController_Platformer controler;
        public Button buttonOut;
        
        public bool finished { get; private set; }
        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay, AudioClip sound, float delayBetween)
        {
            CallSFX();
           // print("CallSFX Method");
           try
           {
               controler.canMove = false;
           }
           catch
           {
               print("No hay Player en esta escena");
           }
            
            textHolder.color = textColor;
            textHolder.font = textFont;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                DialogSound.instace.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }
            audioManager.sounds[placeInAudioManager].volume = 0;
            finalImage.gameObject.SetActive(true);
            buttonOut.gameObject.SetActive(true);
            try
            {
                controler.canMove = true;
            }
            catch
            {
                print("No hay Player en esta escena");
            }
            yield return new WaitForSeconds(delayBetween);
            //yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
            
        }
        
         public void CallSFX()
         {
             audioManager.sounds[placeInAudioManager].volume = 1;
            audioManager.Play("Escribiendo");
        }


        
    }
}


