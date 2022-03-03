using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [SerializeField] private GameObject[] audiosGO;
    

    private void Awake() 
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
            s.source.outputAudioMixerGroup = s.AudioMixerGroup;
        }
        
    }

    private void Start()
    {
        Play("Music");
        try
        {
            Play("QuackPatito");
        }
        catch
        {
            print("AudioQuackPatito");
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds,sound =>sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void CallChangesAudioManager()
    {
        foreach(Sound s in sounds)
        {
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
            s.source.outputAudioMixerGroup = s.AudioMixerGroup;
        }
    }

    public void MuteAll()
    {
        CallChangesAudioManager();
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].mute = true;
        }
        for (int i = 0; i < audiosGO.Length; i++)
        {
            audiosGO[i].GetComponent<AudioSource>().mute = true;
        }
        CallChangesAudioManager();
    }
    public void UnMuteAll()
    {
        CallChangesAudioManager();
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].mute = false;
        }
        for (int i = 0; i < audiosGO.Length; i++)
        {
            audiosGO[i].GetComponent<AudioSource>().mute = false;
        }
        CallChangesAudioManager();
    }
    
    
    
}
