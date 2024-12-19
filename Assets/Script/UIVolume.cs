using UnityEngine;
using UnityEngine.UI;

public class VolumeControlUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider enviroSlider;

    private void Start()
    {
        // Load saved volume settings
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        enviroSlider.value = PlayerPrefs.GetFloat("EnvironmentVolume", 0.5f);

        // Update AudioManager on Start
        UpdateMusicVolume(musicSlider.value);
        UpdateSFXVolume(sfxSlider.value);
        UpdateEnviroVolume(enviroSlider.value);

        // Subscribe to Slider Events
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        enviroSlider.onValueChanged.AddListener(UpdateEnviroVolume);
    }

    public void UpdateMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);  // Save volume settings
    }

    public void UpdateSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);  // Save volume settings
    }

    public void UpdateEnviroVolume(float value)
    {
        AudioManager.Instance.SetEnvironmentVolume(value);
        PlayerPrefs.SetFloat("EnvironmentVolume", value);  // Save volume settings
    }
}