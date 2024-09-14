using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] float minEnemySpawnRadius = 25f;
    [SerializeField] float maxenemySpawnRadius = 50f;
    [SerializeField] float spawnDelay = 5f;
    [SerializeField] List<Enemy> enemies;
    
    GameObject player;
    private void Start() {
        player = GameManager.Instance.player;
        if(player == null) Debug.LogWarning("SpawnManager: Player not found on Start");

        GameManager.OnAfterStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch(state)
        {
            case GameState.Starting:
                InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
                break;
            case GameState.Lose:
                CancelInvoke("SpawnEnemy");
                break;
            default:
                Debug.Log("SpawnManager: Invalid state");
                break;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Enemy enemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];
        Instantiate(enemy, spawnPosition, Quaternion.identity);
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
