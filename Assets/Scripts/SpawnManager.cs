using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerUpPrefab;
    [SerializeField] GameObject _spawnParticle;

    IEnumerator SpawnEnemyRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (!GameManager.Instance.IsGameOver())
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9f, 9f), Random.Range(4f, 5.5f), 0);
            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            GameObject particle = Instantiate(_spawnParticle, spawnPosition, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(1.8f);
            enemy.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            Destroy(particle);
        }
    }

    IEnumerator SpawnPowerUpsRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (!GameManager.Instance.IsGameOver())
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 7f, 0);
            int powerUpToSpawn = Random.Range(0, 6);
            Instantiate(_powerUpPrefab[powerUpToSpawn], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6f, 11f));
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine(1f));
        StartCoroutine(SpawnEnemyRoutine(2.2f));
        StartCoroutine(SpawnEnemyRoutine(3.1f));
        StartCoroutine(SpawnEnemyRoutine(4.3f));
        StartCoroutine(SpawnEnemyRoutine(5.6f));
        StartCoroutine(SpawnPowerUpsRoutine());
    }
}
