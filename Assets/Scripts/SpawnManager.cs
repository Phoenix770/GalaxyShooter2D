using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerUpPrefab;
    bool _isGameOver = false;
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpsRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), 7f, 0);
            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator SpawnPowerUpsRoutine()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 7f, 0);
            int powerUpToSpawn = Random.Range(0, 3);
            Instantiate(_powerUpPrefab[powerUpToSpawn], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6f, 11f));
        }
    }
}
