using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages settings UI bindings for volume controls.
/// Reads current values from SoundController on enable and pushes slider changes back.
/// </summary>
public class SettingsController : MonoBehaviour {
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void OnEnable() {
        SoundController sound = SoundController.Instance;

        masterVolumeSlider.value = sound.masterVolume;
        musicVolumeSlider.value = sound.musicVolume;
        sfxVolumeSlider.value = sound.sfxVolume;

        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    private void OnDisable() {
        masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
    }

    /// <param name="value">Slider value 0-1.</param>
    private void OnMasterVolumeChanged(float value) => SoundController.Instance.SetMasterVolume(value);

    /// <param name="value">Slider value 0-1.</param>
    private void OnMusicVolumeChanged(float value) => SoundController.Instance.SetMusicVolume(value);

    /// <param name="value">Slider value 0-1.</param>
    private void OnSfxVolumeChanged(float value) => SoundController.Instance.SetSfxVolume(value);
}