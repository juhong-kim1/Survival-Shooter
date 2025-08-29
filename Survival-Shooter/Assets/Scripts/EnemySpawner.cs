using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform spawnPoint;

    float timer = 0f;
    float spawnInterval = 5f;
    float enemySpawnNumber = 2f;
    float range = 30f;

    private void Awake()
    {


    }

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
            if (RandomPoint(spawnPoint.position, range, out Vector3 spawnPos))
            {
            }
        }



    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit drop;
            if (NavMesh.SamplePosition(randomPoint, out drop, 1.0f, NavMesh.AllAreas))
            {
                result = drop.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}