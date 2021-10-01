using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume
    {
        get { return _volume; }
        set { 
            _volume = value;
            source.volume = value;
        }
    }
    [Range(0.1f,3f)]public float pitch;
    public bool loop;

    [Range(0f, 1f)] [SerializeField] private float _volume;
    
    [HideInInspector] public AudioSource source;
   
}
