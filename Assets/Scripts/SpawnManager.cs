using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnDelay = 5f;
    [SerializeField]
    private GameObject[] powerUps;
    [SerializeField]
    private float _minSpawnPowerUpDelay = 3f;
    [SerializeField]
    private float _maxSpawnPowerUpDelay = 7f;
    [SerializeField]
    private float _horizontalRange = 8;
    [SerializeField]
    private float _verticalStart = 8f;

    private bool _stopSpawning = false;

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);

        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-_horizontalRange, _horizontalRange);
            Vector3 posToSpawn = new Vector3(randomX, _verticalStart, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);

        while (_stopSpawning == false)
        {
            float spawnDelay = Random.Range(_minSpawnPowerUpDelay, _maxSpawnPowerUpDelay);
            int randomPowerUp = Random.Range(0, 3);
            float randomX = Random.Range(-_horizontalRange, _horizontalRange);
            Vector3 posToSpawn = new Vector3(randomX, _verticalStart, 0);
            
            yield return new WaitForSeconds(spawnDelay);

            Instantiate(powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
        }
    }

    public void StartSpawnRoutines()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
