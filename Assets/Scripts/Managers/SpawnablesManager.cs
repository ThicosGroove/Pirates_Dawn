using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

[DefaultExecutionOrder(5)]
public class SpawnablesManager : MonoBehaviour
{
    public SpawnablesSO spawnablesData;
    [SerializeField] bool SHOULD_SPAWN_EDITOR = true;

    [Header("Player Info")]
    [SerializeField] Transform playerInitialPosition;
    [SerializeField] GameObject PlayerPrefab;

    [Header("Spawn Info")]
    [Range(3, 40)] public float spawnRadius = 10;
    [SerializeField] int MaxEnemyAlive = 5;
    [SerializeField] float EnemySpawnDelay;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();

    private GameObject newPlayer;
    private Vector2 rndPosition;
    private IEnumerator SpawningEnemyCoroutine;

    private void OnEnable()
    {
        ScoreEvents.EnemyDied += TotalEnemyAlive;

        GameplayEvents.GameStart += ShouldSpawnEnemy;
        GameplayEvents.PrePlay += SpawnPlayer;
        GameplayEvents.RestartGame += ClearList;

        PlayerEvents.PlayerDeath += ClearList;

        UtilityEvents.SaveGame += LoadVariables;
    }

    private void OnDisable()
    {
        ScoreEvents.EnemyDied -= TotalEnemyAlive;

        GameplayEvents.GameStart -= ShouldSpawnEnemy;
        GameplayEvents.PrePlay -= SpawnPlayer;
        GameplayEvents.RestartGame -= ClearList;

        PlayerEvents.PlayerDeath -= ClearList;

        UtilityEvents.SaveGame -= LoadVariables;
    }

    void Start()
    {
        SpawningEnemyCoroutine = CreatingEnemy();

        LoadVariables();
    }

    void Update()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PLAYING) return;

        transform.localPosition = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, 0);

        if (enemyList.Count <= MaxEnemyAlive && SpawningEnemyCoroutine == null)
        {
            ShouldSpawnEnemy();
        }
    }

    void LoadVariables()
    {
        EnemySpawnDelay = SaveManager.Instance.LoadFile().SpawnDelay;
    }

    private void SpawnPlayer()
    {
        if (newPlayer != null) return;

        newPlayer = Instantiate(PlayerPrefab, playerInitialPosition.position, Quaternion.identity);

        UtilityEvents.OnSpawnPlayerEvent(newPlayer);
    }

    private void TotalEnemyAlive(GameObject DiedEnemy)
    {
        enemyList.Remove(DiedEnemy);

        if (SpawningEnemyCoroutine == null)
        {
            ShouldSpawnEnemy();
        }
    }

    private void ShouldSpawnEnemy()
    {
        if (GameplayManager.Instance.currentGameState == GameStates.GAMESTART
            || GameplayManager.Instance.currentGameState == GameStates.PLAYING)
        {
            SpawningEnemyCoroutine = CreatingEnemy();
            rndPosition = transform.TransformPoint(Random.insideUnitCircle * spawnRadius);
            RaycastHit2D hit = Physics2D.Raycast(rndPosition, Vector2.up);
            bool OutOfScreen = !GameplayManager.IsObjectOnScreen(rndPosition);

            if (enemyList.Count <= MaxEnemyAlive && SHOULD_SPAWN_EDITOR
                && OutOfScreen && hit.collider != null)
            {
                SpawningEnemyCoroutine = CreatingEnemy();
                StartCoroutine(CreatingEnemy());
            }
            else if (enemyList.Count >= MaxEnemyAlive)
            {   
                return;
            }
            else
            {
                ShouldSpawnEnemy();
            }
        }  
    }

    IEnumerator CreatingEnemy()
    {
        int rndPrefab = (int)Random.Range(0, 2);

        var newEnemy = Instantiate(spawnablesData.enemies[rndPrefab].prefab, rndPosition, Quaternion.identity);
        enemyList.Add(newEnemy);

        yield return new WaitForSeconds(EnemySpawnDelay);
        ShouldSpawnEnemy();
    }

    void ClearList()
    {
        if (SpawningEnemyCoroutine != null)
        {
            StopCoroutine(SpawningEnemyCoroutine);
            SpawningEnemyCoroutine = null;
        }
        StopAllCoroutines();
        enemyList.Clear();
    }
}
