using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

[DefaultExecutionOrder(3)]
public class ScoreManager : Singleton<ScoreManager>
{
    public int currentScore;
    [SerializeField] int _highScore;

    public int highscore
    {
        get
        {
            if (SaveManager.Instance.LoadFile().HighScore != 0)
            {
                return SaveManager.Instance.playerData.HighScore;
            }
            else
            {
                return currentScore;
            }
        }
    }

    private void Start()
    {
        _highScore = highscore;
        currentScore = 0;
    }

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += AddToScore;
        GameplayEvents.RestartGame += ResetScore;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= AddToScore;
        GameplayEvents.RestartGame -= ResetScore;
    }

    public bool CheckForNewHighscore()
    {
        if (_highScore < currentScore)
        {
            SaveManager.Instance.playerData.HighScore = currentScore;
            Debug.LogWarning(currentScore);

            return true;
        }

        return false;
    }

    public void AddToScore(int value)
    {
        currentScore += value;
    }

    private void ResetScore()
    {
        currentScore = 0;
    }
}
