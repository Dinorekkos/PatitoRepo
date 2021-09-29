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
        public Image finalImage;
        public CharacterController_Platformer controler;
       
        public bool finished { get; private set; }
        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay, AudioClip sound, float delayBetween)
        {
            controler.canMove = false;
            textHolder.color = textColor;
            textHolder.font = textFont;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                DialogSound.instace.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }
            finalImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBetween);
            //yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
            controler.canMove = true;

        }
        
        
        
        
        
    
    }
}


