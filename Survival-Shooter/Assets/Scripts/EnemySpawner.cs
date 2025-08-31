using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    //public GameObject[] enemyPrefabs;
    public Transform player;
    public Transform spawnPoint;

    float timer = 0f;
    float spawnInterval = 3f;
    float enemySpawnNumber = 3f;
    float maxRange = 20f;
    private float minRange = 15f;

    public Enemy[] enemyPool;

    private List<Enemy> enemies = new List<Enemy>();



    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            SpawnEnemy();

            timer = 0f;
        }

    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < enemySpawnNumber; ++i)
        {
            if (RandomPoint(spawnPoint.position, maxRange, out Vector3 spawnPos))
            {
                Enemy randomEnemyPrefab;
                int random = Random.Range(0, 100);

                if (random < 40)
                {
                    randomEnemyPrefab = enemyPool[0];
                }
                else if (random < 80)
                {
                    randomEnemyPrefab = enemyPool[1];
                }
                else
                {
                    randomEnemyPrefab = enemyPool[2];
                }

                var enemy = Instantiate(randomEnemyPrefab, spawnPos, spawnPoint.rotation);
                enemies.Add(enemy);

                enemy.OnDeath += () => enemies.Remove(enemy);
                enemy.OnDeath += () => Destroy(enemy.gameObject, 2f);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minRange, range);

            Vector3 spawnPoint = player.position + new Vector3(randomDirection.x * distance, 0, randomDirection.y * distance);

            NavMeshHit drop;
            if (NavMesh.SamplePosition(spawnPoint, out drop, 1.0f, NavMesh.AllAreas))
            {
                result = drop.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}