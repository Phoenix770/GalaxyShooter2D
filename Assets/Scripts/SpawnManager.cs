using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject _tripleShotPrefab;
    bool _isGameOver = false;
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpsRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (!IsGameOver())
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), 7f, 0);
            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator SpawnPowerUpsRoutine()
    {
        while (!IsGameOver())
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 7f, 0);
            Instantiate(_tripleShotPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6f, 11f));
        }
    }

    public void GameOver(bool gameStatus)
    {
        _isGameOver = gameStatus;
    }

    bool IsGameOver()
    {
        return _isGameOver;
    }
}
