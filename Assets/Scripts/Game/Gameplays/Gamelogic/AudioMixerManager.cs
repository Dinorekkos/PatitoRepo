using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private FloatVariables volume;

    private void Update()
    {
        SetAudioMixerVolume();
    }

    public void SetAudioMixerVolume()
    {
        audioMixer.SetFloat("Master", volume.value);
    }
}
