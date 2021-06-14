using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    public Slider VFXSlider;
    public Slider MusicSlider;
    public Button closeButton;
    public Button optionsButton;
    // Start is called before the first frame update
    void Awake()
    {
        VFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        optionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();
        optionsButton.onClick.AddListener(() => PlayAudio("ButtonEffect"));
        optionsButton.onClick.AddListener(() => gameObject.SetActive(true));
        optionsButton.onClick.AddListener(() => optionsButton.gameObject.SetActive(false));

        closeButton.onClick.AddListener(() => PlayAudio("ButtonEffect"));
        closeButton.onClick.AddListener(() => optionsButton.gameObject.SetActive(true));
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void Start()
    {
        AudioManager.audioManager.ReduceVolumeVFX(VFXSlider.value);
        AudioManager.audioManager.ReduceVolumeMusic(MusicSlider.value);
        VFXSlider.onValueChanged.AddListener(AudioManager.audioManager.ReduceVolumeVFX);
        MusicSlider.onValueChanged.AddListener(AudioManager.audioManager.ReduceVolumeMusic);
        VFXSlider.onValueChanged.AddListener(SetVFXValue);
        MusicSlider.onValueChanged.AddListener(SetMusicValue);
        gameObject.SetActive(false);
    }
    public void PlayAudio(string str)
    {
        AudioManager.audioManager.Play(str);
    }

    public void SetVFXValue(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    public void SetMusicValue(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
