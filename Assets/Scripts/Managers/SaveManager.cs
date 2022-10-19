using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using System.IO;

[DefaultExecutionOrder(2)]
public class SaveManager : Singleton<SaveManager>
{
    public PlayerData playerData;

    public int highScore;
    public float spawnDelay;
    public float gameplayTime;

    protected override void Awake()
    {
        base.Awake();

        playerData = new PlayerData();

        if (!File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {
            playerData.HighScore = 0;
            playerData.SpawnDelay = 2f;
            playerData.TotalGameplayTime = 60f;
            playerData.BG_Volume = 1f;
            playerData.SFX_Volume = 0f;
            playerData.BGMusic_mute = false;
            playerData.SFX_mute = false;
            SaveData();
        }

        playerData.HighScore = LoadFile().HighScore;
        playerData.SpawnDelay = LoadFile().SpawnDelay;
        playerData.TotalGameplayTime = LoadFile().TotalGameplayTime;
        playerData.BG_Volume = LoadFile().BG_Volume;
        playerData.SFX_Volume = LoadFile().SFX_Volume;
        playerData.SFX_mute = LoadFile().SFX_mute;
        playerData.BGMusic_mute = LoadFile().BGMusic_mute;      
    }


    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.dataPath + Const.SAVE_FILE_PATH, json);

        UtilityEvents.OnSaveGame();
    }

    public PlayerData LoadFile()
    {
        if (Const.SAVE_FILE_PATH == null) return null;

        string json = File.ReadAllText(Application.dataPath + Const.SAVE_FILE_PATH);
        PlayerData loadPlayerData = JsonUtility.FromJson<PlayerData>(json);
        return loadPlayerData;
    }


    public class PlayerData
    {
        public int HighScore;
        public float SpawnDelay;
        public float TotalGameplayTime;
        public float BG_Volume;
        public float SFX_Volume;
        public bool BGMusic_mute;
        public bool SFX_mute;
    }

}
