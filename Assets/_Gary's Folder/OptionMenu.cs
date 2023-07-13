using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    [Header("Settings Volume Components")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Scrollbar musicScrollbar;
    [SerializeField] private Scrollbar sFXScrollbar;

    [Space]

    [Range(0F, 1F)] [SerializeField] private float saveMusicVaule = 1F; // <-- Once you have a working save system, replace this float to the float of the save so that we can save the volume settings.
    [Range(0F, 1F)] [SerializeField] private float saveSFXVaule = 1F; // <-- This one too!

    void OnEnable()
    {
        SetVolumeSettings();
    }

    private void SetVolumeSettings()
    {
        audioMixer.SetFloat("Music Volume", Mathf.Log10(saveMusicVaule) * 20F);
        audioMixer.SetFloat("SFX Volume", Mathf.Log10(saveSFXVaule) * 20F);

        musicScrollbar.value = saveMusicVaule;
        sFXScrollbar.value = saveSFXVaule;
    }

    public void Music(float volume)
    {
        float number = Mathf.Lerp(0F, 100F, musicScrollbar.value);
        musicScrollbar.GetComponentInChildren<TMP_Text>().text = Mathf.RoundToInt(number) + "%";

        if (musicScrollbar.value == 0F)
        {
            musicScrollbar.value = 0.0001F;
            audioMixer.SetFloat("Music Volume", -80F);
        }
        else
        {
            audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20F);
        }

        saveMusicVaule = musicScrollbar.value;
    }

    public void SFX(float volume)
    {
        float number = Mathf.Lerp(0F, 100F, sFXScrollbar.value);
        sFXScrollbar.GetComponentInChildren<TMP_Text>().text = Mathf.RoundToInt(number) + "%";

        if (sFXScrollbar.value == 0F)
        {
            sFXScrollbar.value = 0.0001F;
            audioMixer.SetFloat("SFX Volume", -80F);
        }
        else
        {
            audioMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20F);
        }

        saveSFXVaule = sFXScrollbar.value;
    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

}
