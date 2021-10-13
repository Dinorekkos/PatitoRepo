using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public float volume
    {
        get { return _volume; }
        set { 
            _volume = value;
            source.volume = value;
        }
    }
    public string name;
    public AudioClip clip;
    public AudioMixerGroup AudioMixerGroup;
    public bool loop;
    [Range(0.1f,3f)]public float pitch;
    [Range(0f, 1f)] [SerializeField] private float _volume;
    
    [HideInInspector] public AudioSource source;
    
    


}
