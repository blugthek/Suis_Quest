using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectMixerGroup;
    [SerializeField] private Sound[] sounds;
    private void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioType.soundEffrct:
                    s.source.outputAudioMixerGroup = soundEffectMixerGroup;
                    break;
                case Sound.AudioType.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }

            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
        
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(Option.musicVolume) * 20);
        soundEffectMixerGroup.audioMixer.SetFloat("Sound Effect Volume", Mathf.Log10(Option.soundEffectVolume) * 20);
    }

}
