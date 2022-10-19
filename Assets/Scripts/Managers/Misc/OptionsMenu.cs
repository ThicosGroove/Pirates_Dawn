using System.Collections;
using System.Collections.Generic;
using GameEvents;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[DefaultExecutionOrder(10)]
public class OptionsMenu : MonoBehaviour
{
    [Header("GameObjects On Scene")]
    [SerializeField] private Slider gameplaySlider;
    [SerializeField] private TMP_Text gameplayTimer;
    [SerializeField] private Slider spawnSlider;
    [SerializeField] private TMP_Text spawnDelay;
    [SerializeField] private Slider BG_VolumeSlider;
    [SerializeField] private Slider SFX_VolumeSlider;
    [SerializeField] private TMP_Text muteSFX_Text;
    [SerializeField] private TMP_Text muteBG_Text;
    [SerializeField] private TMP_Text save_Icon;

    [Header("Options Info")]
    [SerializeField] bool isSFX_Mute = false;
    [SerializeField] bool isBG_Mute = false;
    [SerializeField] private float saveFadeTimer = 1.5f;

    private void Start()
    {
        save_Icon.text = Const.SAVE_ICON;
        save_Icon.color = new Color(save_Icon.color.r, save_Icon.color.g, save_Icon.color.b, 0);

        gameplaySlider.value = SaveManager.Instance.LoadFile().TotalGameplayTime;
        spawnSlider.value = SaveManager.Instance.LoadFile().SpawnDelay;

        isSFX_Mute = SaveManager.Instance.LoadFile().SFX_mute;
        if (isSFX_Mute) muteSFX_Text.text = Const.OPTIONS_TOGGLE_CHAR;
        else muteSFX_Text.text = "";

        isBG_Mute = SaveManager.Instance.LoadFile().BGMusic_mute;
        if (isBG_Mute) muteBG_Text.text = Const.OPTIONS_TOGGLE_CHAR;
        else muteBG_Text.text = "";
    }


    public void GameplayTimeSlider(float newTime)
    {
        UpdateGamePlayTimerUI(newTime);
        SaveManager.Instance.playerData.TotalGameplayTime = newTime;
    }

    private void UpdateGamePlayTimerUI(float newTime)
    {
        float timeOnMinutes = newTime / 60;
        gameplayTimer.text = $"{timeOnMinutes.ToString("F2", CultureInfo.InvariantCulture)} Minutes";
    }

    public void SpawnDelaySlider(float newDelay)
    {
        UpdateSpawnDelayTimerUI(newDelay);
        SaveManager.Instance.playerData.SpawnDelay = newDelay;
    }
    private void UpdateSpawnDelayTimerUI(float newDelay)
    {
        spawnDelay.text = $"{newDelay.ToString("F2", CultureInfo.InvariantCulture)} Seconds";
    }


    public void ClickOnSaveButton()
    {
        SaveManager.Instance.SaveData();
        StartCoroutine(SaveIconFade());
    }

    IEnumerator SaveIconFade()
    {
        float r = save_Icon.color.r;
        float g = save_Icon.color.g;
        float b = save_Icon.color.b;
        float alpha = save_Icon.color.a;
        save_Icon.color = new Color(r, g, b, 255);

        yield return new WaitForSeconds(1f);

        for (float t = 0; t < 1f; t += Time.deltaTime / saveFadeTimer)
        {
            Color newColor = new Color(r, g, b, Mathf.Lerp(alpha, 0, t));
            save_Icon.color = newColor;

            yield return null;
        }
    }


    public void ToggleMuteSFX()
    {
        isSFX_Mute = !isSFX_Mute;

        if (isSFX_Mute)
        {
            muteSFX_Text.text = Const.OPTIONS_TOGGLE_CHAR;
            UtilityEvents.OnMuteSFXToggle(isSFX_Mute);
            SaveManager.Instance.playerData.SFX_mute = isSFX_Mute;
        }
        else
        {
            muteSFX_Text.text = "";
            UtilityEvents.OnMuteSFXToggle(isSFX_Mute);
            SaveManager.Instance.playerData.SFX_mute = isSFX_Mute;
        }
    }

    public void ToggleMuteBGMusic()
    {
        isBG_Mute = !isBG_Mute;

        if (isBG_Mute)
        {
            muteBG_Text.text = Const.OPTIONS_TOGGLE_CHAR;
            UtilityEvents.OnBGMusicToggle(isBG_Mute);
            SaveManager.Instance.playerData.BGMusic_mute = isBG_Mute;
        }
        else
        {
            muteBG_Text.text = "";
            UtilityEvents.OnBGMusicToggle(isBG_Mute);
            SaveManager.Instance.playerData.BGMusic_mute = isBG_Mute;
        }

    }
}
