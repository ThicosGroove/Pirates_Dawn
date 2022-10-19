using UnityEngine;
using UnityEngine.Events;

// Namespace que centraliza todos os eventos do jogo.
namespace GameEvents
{
    public static class PlayerEvents
    {
        public static event UnityAction PlayerHit;
        public static void OnPlayerHit() => PlayerHit?.Invoke();

        public static event UnityAction PlayerDeath;
        public static void OnPlayerDeath() => PlayerDeath?.Invoke();
    }

    public static class ScoreEvents
    {
        public static event UnityAction<GameObject> EnemyDied;
        public static void OnEmenyDied(GameObject enemy) => EnemyDied?.Invoke(enemy);

        public static event UnityAction<int> ScoreGained;
        public static void OnScoreGained(int value) => ScoreGained?.Invoke(value);
    }

    public static class GameplayEvents
    {
        public static event UnityAction MainMenu;
        public static void OnMainMenu() => MainMenu?.Invoke();
        
        public static event UnityAction PrePlay;
        public static void OnPrePlay() => PrePlay?.Invoke();

        public static event UnityAction GameStart;
        public static void OnGameStart() => GameStart?.Invoke();

        public static event UnityAction RestartGame;
        public static void OnRestartGame() => RestartGame?.Invoke();

        public static event UnityAction RespawnPlayer;
        public static void OnRespawnPlayer() => RespawnPlayer?.Invoke();

    }

    public static class UtilityEvents
    {
        public static event UnityAction<GameObject> SpawnPlayerEvent;
        public static void OnSpawnPlayerEvent(GameObject player) => SpawnPlayerEvent?.Invoke(player);

        public static event UnityAction SaveGame;
        public static void OnSaveGame() => SaveGame?.Invoke();

        public static event UnityAction GamePauseToggle;
        public static void OnGamePauseToggle() => GamePauseToggle?.Invoke();

        public static event UnityAction<bool> SFXToggle;
        public static void OnMuteSFXToggle(bool isMute) => SFXToggle?.Invoke(isMute);

        public static event UnityAction<bool> BGMusicToggle;
        public static void OnBGMusicToggle(bool isMute) => BGMusicToggle?.Invoke(isMute);
    }
}