using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;
    public static AudioManager audioManager;


    void Awake()
    {
        if (audioManager == null)
            audioManager = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }

    }
    private void Start()
    {
        Play("ThemeMenu");
    }
    public void Play(string name)
    {
        Sound s = sounds.Find(x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + "not found!");
            return;
        }

        s.source.pitch = s.pitch;

        s.source.volume = s.volume;
        s.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = sounds.Find(x => x.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Stop();
    }

    public void ReduceVolumeVFX(float vol)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundType == Sound.SoundType.VFX)
            {
                s.volume = vol;
                s.source.volume = vol;
            }
        }
    }
    public void ReduceVolumeMusic(float vol)
    {
        Debug.Log(vol);
        foreach (Sound s in sounds)
        {
            if (s.soundType == Sound.SoundType.MUSIC)
            {
                s.volume = vol;
                s.source.volume = vol;
            }
        }
    }
}
