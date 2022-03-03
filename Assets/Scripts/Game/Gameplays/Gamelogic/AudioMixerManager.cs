using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [Header("Slider Audio UI")] 
    [SerializeField] private Slider mainAudioSlider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private FloatVariables volume;

    private void Start()
    {
        mainAudioSlider.value = volume.value;
    }

    private void Update()
    {
        SetAudioMixerVolume();
    }
    

    public void SetAudioMixerVolume()
    {
        audioMixer.SetFloat("Master", volume.value);
    }
}
