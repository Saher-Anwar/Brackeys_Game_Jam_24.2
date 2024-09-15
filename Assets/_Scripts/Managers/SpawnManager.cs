using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] float minEnemySpawnRadius = 25f;
    [SerializeField] float maxenemySpawnRadius = 50f;
    [SerializeField] float spawnDelay = 5f;
    [SerializeField] List<Enemy> enemies;

    [Header("Collectible Spawn Settings")]
    [SerializeField] float collectibleSpawnDelay = 5f;
    [SerializeField] List<Collectible> collectibles;
    [SerializeField] int maxCollectiblesAllowed = 3;
    
    int collectibleCount = 0;
    public PanicBar panicBar;
    GameObject player;

    private void Start() {
        player = GameManager.Instance.player;
        if(player == null) Debug.LogWarning("SpawnManager: Player not found on Start");

        GameManager.OnAfterStateChanged += OnGameStateChanged;
        Collectible.OnCollectibleCollected += OnCollectibleCollected;
    }

    private void OnCollectibleCollected()
    {
        collectibleCount -= 1;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch(state)
        {
            case GameState.Starting:
                InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
                InvokeRepeating("SpawnCollectible", 0f, collectibleSpawnDelay);
                break;
            case GameState.Lose:
                CancelInvoke("SpawnEnemy");
                CancelInvoke("SpawnCollectible");
                break;
            default:
                Debug.Log("SpawnManager: Invalid state");
                break;
        }
    }

    void SpawnCollectible()
    {
        if(collectibleCount >= maxCollectiblesAllowed) return;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        Collectible collectible = collectibles[UnityEngine.Random.Range(0, collectibles.Count)];
        Instantiate(collectible, spawnPosition, Quaternion.identity);
        collectibleCount += 1;
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Enemy enemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];
        Instantiate(enemy, spawnPosition, Quaternion.identity);
        panicBar.IncreaseFillAmount();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        float distance = UnityEngine.Random.Range(minEnemySpawnRadius, maxenemySpawnRadius);

        // Convert polar coordinates (angle, distance) to Cartesian coordinates (x, y)
        float spawnX = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float spawnY = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        // Return the spawn position relative to the player's position (or any reference point)
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f); 

        // Adjust spawn position relative to the player's position (if needed)
        spawnPosition += player.transform.position;  

        return spawnPosition;
    }

    private void OnDrawGizmosSelected() {
        if(player == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.transform.position, minEnemySpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, maxenemySpawnRadius);
    }
}
