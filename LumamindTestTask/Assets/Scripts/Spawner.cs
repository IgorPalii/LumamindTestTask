using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const int MAX_ENEMY_SPAWNED = 5, MAX_SPAWN_OFSET_Y = 3, MAX_SPAWN_OFSET_X = 3;
    [SerializeField]
    private GameObject enemyPref;
    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        Enemy.enemyDeath += RespawnEnemy;
        for (int i = 0; i < MAX_ENEMY_SPAWNED; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var x = Random.Range(transform.position.x - MAX_SPAWN_OFSET_X, transform.position.x + MAX_SPAWN_OFSET_X);
        var y = Random.Range(transform.position.y - MAX_SPAWN_OFSET_Y, transform.position.y + MAX_SPAWN_OFSET_Y);
        var spawnPos = new Vector2(x, y);

        var enemy = Instantiate(enemyPref, spawnPos, Quaternion.identity);
        enemies.Add(enemy);
    }

    private void RespawnEnemy(Enemy enemy)
    {
        enemies.Remove(enemy.gameObject);
        if (enemies.Count < MAX_ENEMY_SPAWNED) SpawnEnemy();
    }
}
