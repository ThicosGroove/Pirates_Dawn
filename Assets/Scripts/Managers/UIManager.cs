using UnityEngine;
using TMPro;
using GameEvents;

// Execution order maior do que o normal, para garantir que o script sera executado DEPOIS do player.
// Manager que controla todos os objetos de texto.
[DefaultExecutionOrder(4)]
public class UIManager : MonoBehaviour
{
    [Header("All Menus On Scene")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject prePlayMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    [Header("Text")]
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameover_ScoreText;
    [SerializeField] private TMP_Text gameover_NewHighScoreText;
    [SerializeField] private TMP_Text gameover_HighScoreText;

    private void OnEnable()
    {
        ScoreEvents.ScoreGained += UpdateScoreUI;

        PlayerEvents.PlayerDeath += OnGameOver;

        UtilityEvents.GamePauseToggle += OnGamePauseToggle;
    }

    private void OnDisable()
    {
        ScoreEvents.ScoreGained -= UpdateScoreUI;

        PlayerEvents.PlayerDeath -= OnGameOver;

        UtilityEvents.GamePauseToggle -= OnGamePauseToggle;
    }

    private void Start()
    {
        GoToMainMenu();
    }

    #region Menu

    private void OnPrePlay()
    {
        ToggleAllMenuOff();
        prePlayMenu.SetActive(true);
    }

    private void OnGameStart()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PLAYING) return;

        ToggleAllMenuOff();
        ToggleGameplayUI(true);
        UpdateScoreUI();
    }

    private void OnOptionsMenu()
    {
        if (GameplayManager.Instance.currentGameState == GameStates.OPTION_MENU)
        {
            ToggleAllMenuOff();
            optionsMenu.SetActive(true);
        }
    }

    private void GoToMainMenu()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.MAIN_MENU) return;

        ToggleAllMenuOff();
        mainMenu.SetActive(true);
    }

    private void OnGamePauseToggle()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PAUSED
            || GameplayManager.Instance.currentGameState != GameStates.PLAYING)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    private void OnGameOver()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.GAMEOVER) return;

        ToggleAllMenuOff();
        GameOverUI();

        gameOverMenu.SetActive(true);
        gameover_ScoreText.gameObject.SetActive(true);
        gameover_NewHighScoreText.gameObject.SetActive(false);

        if (ScoreManager.Instance.CheckForNewHighscore())
        {
            gameover_NewHighScoreText.gameObject.SetActive(true);
            SaveManager.Instance.SaveData();
        }
    }

    private void ToggleAllMenuOff()
    {
        mainMenu.SetActive(false);
        ToggleGameplayUI(false);
        optionsMenu.SetActive(false);
        prePlayMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    #endregion

    #region UI

    private void ToggleGameplayUI(bool active)
    {
        scoreText.gameObject.SetActive(active);
    }

    private void UpdateScoreUI(int _ = 0)
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PLAYING) return;

        scoreText.gameObject.SetActive(true);

        scoreText.text = $"$ {ScoreManager.Instance.currentScore}";
    }

    private void GameOverUI()
    {
        gameover_ScoreText.text = $"$\n{ScoreManager.Instance.currentScore}";
        gameover_HighScoreText.text = $"Highscore:\n{ScoreManager.Instance.highscore}";
    }

    #endregion

    #region Buttons

    public void ClickStartGame()
    {
        GameplayManager.Instance.UpdateGameState(GameStates.PREPLAY);
        OnPrePlay();
    }

    public void PressToStart()
    {
        GameplayManager.Instance.UpdateGameState(GameStates.GAMESTART);
        OnGameStart();
    }

    public void PressOptions()
    {
        GameplayManager.Instance.UpdateGameState(GameStates.OPTION_MENU);
        OnOptionsMenu();
    }

    public void PressRestartOnPause()
    {
        GameplayEvents.OnRestartGame();
        GameplayManager.Instance.UpdateGameState(GameStates.MAIN_MENU);
        GoToMainMenu();
    }

    public void pressBackToMenu()
    {
        GameplayManager.Instance.UpdateGameState(GameStates.MAIN_MENU);
        GoToMainMenu();
    }

    public void PressBackToPause()
    {
        GameplayManager.Instance.UpdateGameState(GameStates.PAUSED);
        ToggleAllMenuOff();
        pauseMenu.SetActive(true);
    }

    public void PressMenuGameOver()
    {
        GameplayManager.Instance.UpdateGameState(GameStates.MAIN_MENU);
        GameplayEvents.OnRestartGame();
        GoToMainMenu();
    }

    public void PressPlayAgain()
    {
        GameplayEvents.OnRestartGame();
        GameplayEvents.OnPrePlay();
        GameplayManager.Instance.UpdateGameState(GameStates.PREPLAY);
        OnPrePlay();
    }

    public void PressExitGame()
    {
        Application.Quit();
    }

    #endregion
}
