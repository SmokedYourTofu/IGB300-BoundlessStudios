using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer masterMixer;

    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MusicPref = "MusicVolume";
    private const string SfxPref = "SFXVolume";

    private void Awake()
    {
        // Implementing Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load saved volumes or set to default (1.0f)
        float musicVolume = PlayerPrefs.GetFloat(MusicPref, 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat(SfxPref, 1.0f);

        // Set the sliders to the loaded values
        if (musicSlider != null)
            musicSlider.value = musicVolume;

        if (sfxSlider != null)
            sfxSlider.value = sfxVolume;

        // Apply the volumes to the AudioMixer
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // Add listeners to the sliders
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        // AudioMixer expects volume in decibels, convert from linear [0.0001, 1] to decibels [-80, 0]
        float dB;
        if (volume > 0)
            dB = Mathf.Log10(volume) * 20;
        else
            dB = -80f; // Mute

        masterMixer.SetFloat("MusicVolume", dB);

        // Save the volume setting
        PlayerPrefs.SetFloat(MusicPref, volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dB;
        if (volume > 0)
            dB = Mathf.Log10(volume) * 20;
        else
            dB = -80f; // Mute

        masterMixer.SetFloat("SFXVolume", dB);

        // Save the volume setting
        PlayerPrefs.SetFloat(SfxPref, volume);
    }
}
