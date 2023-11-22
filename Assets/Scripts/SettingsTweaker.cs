using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsTweaker : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Slider fontSlider;
    private bool initialized = false;
    public TMP_FontAsset scalableFont;

    public float defaultFontScale = 0.5f;
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }


    public void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.6f) * masterSlider.maxValue;
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.7f) * masterSlider.maxValue;
        sfxSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.7f) * masterSlider.maxValue;
        fontSlider.value = map(PlayerPrefs.GetFloat("FontScale", 0.5f), defaultFontScale, 1 + defaultFontScale, fontSlider.minValue, fontSlider.maxValue);
        OnTextScaleValueChanged(fontSlider);
       initialized = true;
    }

    public void OnSliderValueChanged(Slider slider)
    {
        if (!initialized)
        {
            return;
        }
        if (AudioManager.instance == null)
        {
            return;
        }
        AudioManager.instance.PlaySound("Click", 1,0.5f+ slider.value / slider.maxValue);
        AudioManager.instance.changeVolume(slider.gameObject.name, slider.value / slider.maxValue) ;
    }

    public void OnTextScaleValueChanged(Slider s)
    {

        SettingsManager.instance.textScaleMultiplier = s.value / s.maxValue + defaultFontScale;
        var faceInfo = scalableFont.faceInfo;
        faceInfo.scale = SettingsManager.instance.textScaleMultiplier;
        scalableFont.faceInfo = faceInfo;
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        AudioManager.instance?.PlaySound("Click", 1, 0.5f + s.value / s.maxValue);
        PlayerPrefs.SetFloat("FontScale", SettingsManager.instance.textScaleMultiplier);
    }
}
