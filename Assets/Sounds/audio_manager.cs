using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound{
    public string name;
    public string tag;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}

public class audio_manager : MonoBehaviour
{
    public Sound[] sounds;
    
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

    }

    public Sound findsound(string name){
        Sound s = Array.Find(sounds, sound => sound.name==name);
        return s;
    }
    public void play(string name)
    {
         Sound s = Array.Find(sounds, sound => sound.name==name);
         s.source.Play();
    }
    public void set_volume(string name, float v){
        Sound s = Array.Find(sounds, sound => sound.name==name);
        s.volume = v;
        s.source.volume = s.volume;
    }
    public void set_effect_volume(float v){
        foreach(Sound s in sounds){
            if(s.tag == "effect"){
                s.volume = v;
                s.source.volume = s.volume;
            }
        }
    }
}
