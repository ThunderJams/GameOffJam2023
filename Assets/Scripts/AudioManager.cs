using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public  List<AudioClip> sfxList;
    public  List<AudioClip> musicList;

    public AudioSource musicAudioSource;

    public static AudioManager instance;

    private float masterVolume = 1;
    private float soundEffectsVolume = 1;
    private float musicVolume = 1;

    public AudioSource soundEffectsAudioSource;

    public int pooledSourcesCount = 10;
    int currentSourceIndex = 0;
    public List<AudioSource> pooledSoundEffectAudioSources;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);


        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.6f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        soundEffectsVolume= PlayerPrefs.GetFloat("SoundVolume", 0.7f);
        UpdateSoundValues();
        PlayMusic("GameMusic");
        for (int i = 0; i < pooledSourcesCount; i++)
        {
            AudioSource audioSource = soundEffectsAudioSource.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            pooledSoundEffectAudioSources.Add(audioSource);
        }

    }

    public void PlayMusic()
    {
        musicAudioSource.Play();   

    }

    public void changeVolume(string affected,  float audioValue)
    {
        switch (affected)
        {
            case "Master":
                masterVolume = audioValue;
                PlayerPrefs.SetFloat("MasterVolume", audioValue);

                break;
            case "Sound Effects":
                soundEffectsVolume  = audioValue;
                PlayerPrefs.SetFloat("SoundVolume", audioValue);
                break;
            case "Music":
                musicVolume  = audioValue;
                PlayerPrefs.SetFloat("MusicVolume", audioValue);
                break;
        }
        UpdateSoundValues();
    }
    public void UpdateSoundValues()
    {

        musicAudioSource.volume = masterVolume * musicVolume;
        soundEffectsAudioSource.volume = masterVolume * soundEffectsVolume;
        foreach (AudioSource audioSource in pooledSoundEffectAudioSources)
        {
            audioSource.volume = masterVolume * soundEffectsVolume;
        }
    }
    public void PlaySound(string soundName, float volumeScale = 1, float pitch = 1)
    {
        if (SettingsManager.instance.settingsValues.MuteSound)
        {
            return;
        }
        AudioClip sfx = sfxList.Where((AudioClip x) => x.name.ToUpper() == soundName.ToUpper()).FirstOrDefault();
        if (sfx != null)
        {
            pooledSoundEffectAudioSources[currentSourceIndex].pitch = pitch;
            pooledSoundEffectAudioSources[currentSourceIndex].PlayOneShot(sfx, volumeScale);
        }
        else
        {
            Debug.LogError("Invalid audio clip name for " + soundName);
        }
        currentSourceIndex = (currentSourceIndex + 1)%pooledSourcesCount;
    }
    public void PlayMusic(string musicName)
    {

        if (SettingsManager.instance.settingsValues.MuteSound)
        {
            return;
        }

        AudioClip music = musicList.Where((AudioClip x) => x.name.ToUpper() == musicName.ToUpper()).First();
        if (music != null)
        {
            musicAudioSource.clip = music;
            musicAudioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid audio clip name for " + musicName);
        }
    }
}
