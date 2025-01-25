using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        float bgmValue, sfxValue;

        if (audioMixer.GetFloat("BGMVolume", out bgmValue))
        {
            bgmSlider.value = Mathf.Pow(10, bgmValue / 20);
        }

        if (audioMixer.GetFloat("SFXVolume", out sfxValue))
        {
            sfxSlider.value = Mathf.Pow(10, sfxValue / 20);
        }

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float value)
    {

        if (value <= 0.001f) 
        {
            audioMixer.SetFloat("BGMVolume", -80f);
        }
        else
        {
            float volume = Mathf.Log10(value) * 20;
            audioMixer.SetFloat("BGMVolume", volume);
        }
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.001f) 
        {
            audioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            float volume = Mathf.Log10(value) * 20;
            audioMixer.SetFloat("SFXVolume", volume);
        }
    }
}
