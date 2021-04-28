using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    bool _isGameOver = false;
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
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

    public void GameOver(bool gameStatus)
    {
        _isGameOver = gameStatus;
    }

    bool IsGameOver()
    {
        return _isGameOver;
    }
}
