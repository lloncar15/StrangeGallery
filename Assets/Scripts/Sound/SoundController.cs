using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : PersistentSingleton<SoundController> {
    [Header("Audio Sources")]
    [SerializeField] public List<AudioSourceData> audioSources;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    #region Audio Sources

    public void AddAudioSource(AudioSourceData audioSource) {
        audioSources.Add(audioSource);
    }

    #endregion

    #region Sound Play Controls

    public void PlayMusic(AudioClip clip) {
        
    }

    /// <summary>
    /// Plays an audio clip from a given audio source
    /// </summary>
    /// <param name="source">Audio source to play the sound from</param>
    /// <param name="clip">Clip to play</param>
    public void PlaySound(AudioSource source, AudioClip clip) {
        if (!source || !clip)
            return;
        
        source.volume = GetSfxVolume();
        source.PlayOneShot(clip);
    }

    #endregion

    #region Volume Controls

    private float GetMusicVolume() {
        return masterVolume * musicVolume;
    }

    private float GetSfxVolume() {
        return masterVolume * sfxVolume;
    }

    private float GetVolume(AudioType audioType) {
        return audioType ==  AudioType.Music ? GetMusicVolume() : GetSfxVolume();
    }

    public void SetMasterVolume(float volume) {
        masterVolume = Mathf.Clamp01(volume);
        UpdateSoundVolume();
    }

    public void SetMusicVolume(float volume) {
        musicVolume = Mathf.Clamp01(volume);
        UpdateSoundVolume();
    }

    public void SetSfxVolume(float volume) {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSoundVolume();
    }

    #endregion

    #region Utility

    private void UpdateSoundVolume() {
        foreach (AudioSourceData audioSource in audioSources) {
            UpdateAudioSourceVolume(audioSource);
        }
    }

    private void UpdateAudioSourceVolume(AudioSourceData audioSource) {
        audioSource.source.volume = GetVolume(audioSource.audioType);
    }

    #endregion
}

public enum AudioType {
    Music,
    SoundEffect
}

[Serializable]
public struct AudioSourceData {
    [SerializeField] public string name;
    [SerializeField] public AudioType audioType;
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public AudioSource source;
}