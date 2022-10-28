using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using GameEvents;

public enum GameStates
{
    GAMESTART = 0,
    PAUSED = 1,
    PREPLAY = 2,
    GAMEOVER = 3,
    MAIN_MENU = 4,
    OPTION_MENU = 5,
    PLAYING = 6
}

// Manager que controla a gameplay e providencia alguns métodos utilitários.
[DefaultExecutionOrder(1)]
public class GameplayManager : Singleton<GameplayManager>
{
    public GameStates currentGameState = GameStates.MAIN_MENU;
    public static event Action<GameStates> OnGameStateChanged;

    // Retorna verdadeiro se o ponto é visível pela câmera, e falso caso não seja.
    public static bool IsObjectOnScreen(Vector3 worldPosition)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        if (viewportPosition.x < 0 || viewportPosition.x > 1
            || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            return false;
        }

        return true;
    }

    protected override void Awake()
    {
        base.Awake();

        UtilityEvents.GamePauseToggle += Instance.OnGamePauseToggle;
    }

    private void OnApplicationQuit()
    {

        UtilityEvents.GamePauseToggle -= Instance.OnGamePauseToggle;
    }

    private void Start()
    {
        currentGameState = GameStates.MAIN_MENU;
    }

    public void UpdateGameState(GameStates newState)
    {
        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (currentGameState)
        {
            case GameStates.MAIN_MENU:
                MainMenu();
                break;
            case GameStates.PREPLAY:
                PrePlay();
                break;
            case GameStates.PLAYING:
                break;
            case GameStates.OPTION_MENU:
                break;
            case GameStates.PAUSED:
                break;
            case GameStates.GAMESTART:
                GameStart();
                break;
            case GameStates.GAMEOVER:
                GameOver();
                break;
            default:
                break;
        }
    }

    void MainMenu()
    {
        GameplayEvents.OnMainMenu();
        Time.timeScale = 1;
    }

    void PrePlay()
    {
        GameplayEvents.OnPrePlay();
    }

    void GameStart()
    {
        GameplayEvents.OnGameStart();
        UpdateGameState(GameStates.PLAYING);
    }

    void GameOver()
    {
        PlayerEvents.OnPlayerDeath();
    }

    void OnGamePauseToggle()
    {
        if (currentGameState == GameStates.PREPLAY
        || currentGameState == GameStates.GAMEOVER
        || currentGameState == GameStates.MAIN_MENU) return;

        if (currentGameState == GameStates.PAUSED)
        {
            UpdateGameState(GameStates.PLAYING);
        }
        else
        {
            UpdateGameState(GameStates.PAUSED);
        }

        Cursor.lockState = Cursor.lockState == CursorLockMode.Confined ? CursorLockMode.None : CursorLockMode.Confined;
        Time.timeScale = 1 - Time.timeScale; // Se timescale for 0, retorna 1, caso seja 1, retorna 0.
    }
}
