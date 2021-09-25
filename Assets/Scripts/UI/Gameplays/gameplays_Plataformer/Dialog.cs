using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogSystem
{
    public class Dialog : DialogManager
    {
        [Header("Text Options")]
        private Text textHolder;
       
        [SerializeField] private string input;
        [SerializeField] private AudioClip sound;
        [SerializeField] private Color txtColor;
        [SerializeField] private Font txtFont;

        [Header("Time Parameters")]
        [SerializeField] private float delay;
        [SerializeField] private float delayBetween;
        
        [SerializeField] private Sprite charSprite;
        [SerializeField] private Image imageHolder;
        
        private void Awake()
        {
            textHolder = GetComponent<Text>();
            textHolder.text = "";
            imageHolder.sprite = charSprite;
            imageHolder.preserveAspect = true;
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, txtColor, txtFont, delay,sound, delayBetween));
        }
        
    }
}

