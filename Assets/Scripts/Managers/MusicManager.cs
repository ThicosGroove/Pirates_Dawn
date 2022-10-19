using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Classe herdeira de uma singleton,
// para a música não reiniciar ao recarregar a cena.
[DefaultExecutionOrder(10)]
public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider bgSlider;
    [SerializeField] Slider sfxSlider;

    private bool isBG_Muted = false;
    private bool isSFX_Muted;
    private float BG_Volume;
    private float SFX_Volume;

    protected override void Awake()
    {
        base.Awake();

        bgSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        BG_Volume = SaveManager.Instance.LoadFile().BG_Volume;
        SFX_Volume = SaveManager.Instance.LoadFile().SFX_Volume;

        isBG_Muted = SaveManager.Instance.LoadFile().BGMusic_mute;
        isSFX_Muted = SaveManager.Instance.LoadFile().SFX_mute;

        LoadValues();
    }

    private void LoadValues()
    {
        bgSlider.value = BG_Volume;
        sfxSlider.value = SFX_Volume;
    }

    private void Update()
    {
        if (!isBG_Muted)
            UpdateBGVolume();     
        else
            Mute();

        if (!isSFX_Muted)
            UpdateSFXVolume();
        else
            Mute();
    }

    private void UpdateBGVolume()
    {
        mixer.SetFloat(Const.BGM_VOLUME_AUDIOMIXER_KEY, Mathf.Log10(BG_Volume) * 20);

    }

    private void UpdateSFXVolume()
    {
        mixer.SetFloat(Const.SFX_VOLUME_AUDIOMIXER_KEY, Mathf.Log10(SFX_Volume) * 20);

    }

    private void Mute()
    {
        if (isBG_Muted)
            mixer.SetFloat(Const.BGM_VOLUME_AUDIOMIXER_KEY, -80f);
        else
            mixer.SetFloat(Const.BGM_VOLUME_AUDIOMIXER_KEY, Mathf.Log10(BG_Volume) * 20);


        if (isSFX_Muted)
            mixer.SetFloat(Const.SFX_VOLUME_AUDIOMIXER_KEY, -80f);
        else
            mixer.SetFloat(Const.SFX_VOLUME_AUDIOMIXER_KEY, Mathf.Log10(SFX_Volume) * 20);

    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(Const.BGM_VOLUME_AUDIOMIXER_KEY, Mathf.Log10(value) * 20);
        SaveManager.Instance.playerData.BG_Volume = value;
        BG_Volume = value;
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(Const.SFX_VOLUME_AUDIOMIXER_KEY, Mathf.Log10(value) * 20);
        SaveManager.Instance.playerData.SFX_Volume = value;
        SFX_Volume = value;
    }

    public void ToggleMuteBGMusic()
    {
        isBG_Muted = !isBG_Muted;
    }

    public void ToggleMuteSFX()
    {
        isSFX_Muted = !isSFX_Muted;
    }


}
