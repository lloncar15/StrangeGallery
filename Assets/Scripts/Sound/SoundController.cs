using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : PersistentSingleton<SoundController> {
    [Header("Registered Audio Sources")]
    [SerializeField] private List<RegisteredAudioSource> registeredSources;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    #region Registration

    /// <summary>
    /// Registers a persistent audio source (music, ambient, etc.) for volume management.
    /// Registered sources will have their volume updated automatically when settings change.
    /// </summary>
    /// <param name="source">The audio source data to register</param>
    public void Register(RegisteredAudioSource source) {
        registeredSources.Add(source);
        UpdateRegisteredSourceVolume(source);
    }

    /// <summary>
    /// Unregisters a persistent audio source from volume management.
    /// </summary>
    /// <param name="source">The audio source data to unregister</param>
    public void Unregister(RegisteredAudioSource source) {
        registeredSources.Remove(source);
    }

    #endregion

    #region Playback

    /// <summary>
    /// Plays a music clip. (Not yet implemented)
    /// </summary>
    /// <param name="clip">The music clip to play</param>
    public void PlayMusic(AudioClip clip) {
        // TODO: Implement music playback
    }

    /// <summary>
    /// Plays a one-shot sound effect from the given audio source at the current SFX volume.
    /// The source does not need to be registered.
    /// </summary>
    /// <param name="source">Audio source to play the sound from</param>
    /// <param name="clip">Clip to play</param>
    public void PlayOneShotSfx(AudioSource source, AudioClip clip) {
        if (!source || !clip)
            return;

        source.PlayOneShot(clip, GetSfxVolume());
    }

    #endregion

    #region Volume Controls

    /// <summary>
    /// Returns the effective music volume (master * music).
    /// </summary>
    public float GetMusicVolume() {
        return masterVolume * musicVolume;
    }

    /// <summary>
    /// Returns the effective SFX volume (master * sfx).
    /// </summary>
    public float GetSfxVolume() {
        return masterVolume * sfxVolume;
    }

    /// <summary>
    /// Sets the master volume and updates all registered sources.
    /// </summary>
    /// <param name="volume">Volume value between 0 and 1</param>
    public void SetMasterVolume(float volume) {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllRegisteredSourceVolumes();
    }

    /// <summary>
    /// Sets the music volume and updates all registered sources.
    /// </summary>
    /// <param name="volume">Volume value between 0 and 1</param>
    public void SetMusicVolume(float volume) {
        musicVolume = Mathf.Clamp01(volume);
        UpdateAllRegisteredSourceVolumes();
    }

    /// <summary>
    /// Sets the SFX volume and updates all registered sources.
    /// </summary>
    /// <param name="volume">Volume value between 0 and 1</param>
    public void SetSfxVolume(float volume) {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateAllRegisteredSourceVolumes();
    }

    #endregion

    #region Internal

    /// <summary>
    /// Returns the effective volume for a given sound type.
    /// </summary>
    /// <param name="soundType">The type of sound</param>
    /// <returns>The computed volume</returns>
    private float GetVolume(SoundType soundType) {
        return soundType == SoundType.Music ? GetMusicVolume() : GetSfxVolume();
    }

    /// <summary>
    /// Updates the volume of all registered audio sources based on current settings.
    /// </summary>
    private void UpdateAllRegisteredSourceVolumes() {
        foreach (RegisteredAudioSource source in registeredSources) {
            UpdateRegisteredSourceVolume(source);
        }
    }

    /// <summary>
    /// Updates the volume of a single registered audio source.
    /// </summary>
    /// <param name="source">The registered source to update</param>
    private void UpdateRegisteredSourceVolume(RegisteredAudioSource source) {
        source.source.volume = GetVolume(source.soundType);
    }

    #endregion
}

public enum SoundType {
    Music,
    SoundEffect
}

[Serializable]
public struct RegisteredAudioSource : IEquatable<RegisteredAudioSource> {
    [SerializeField] public string name;
    [SerializeField] public SoundType soundType;
    [SerializeField] public AudioSource source;

    public bool Equals(RegisteredAudioSource other) {
        return source == other.source;
    }

    public override bool Equals(object obj) {
        return obj is RegisteredAudioSource other && Equals(other);
    }

    public override int GetHashCode() {
        return source != null ? source.GetHashCode() : 0;
    }
}