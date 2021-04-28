using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 15f;
    [SerializeField] GameObject _explosionPrefab;
    SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager is NULL!");
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            DestroyAsteroid();
        }
        else if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player == null)
                return;

            player.DestroyPlayer();
        }

    }

    void DestroyAsteroid()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager.StartSpawning();
        Destroy(gameObject, 0.25f);
        Destroy(explosion, 3f);
    }
}
