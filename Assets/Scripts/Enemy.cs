using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class Enemy : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float _speed = 4f;
    bool _isDead = false;

    [Header("Score")]
    [SerializeField] int _scoreAmount = 10;

    [Header("Spawning & Relocation")]
    [SerializeField] GameObject _spawnParticle;
    bool _isBeingRelocated;

    [Header("Shooting")]
    [SerializeField] GameObject _enemyLaserPrefab;
    float _fireRate = 3f;
    float _canFire = -1f;

    [Header("Components")]
    Player _player;
    Animator _anim;
    AudioSource _audioSource;

    [Header("Enemy Shields")]
    [SerializeField] GameObject _shields;
    bool _isShieldActive = false;

    #endregion

    private void Start()
    {
        InitializeComponents();
        SpawnWithShield();
    }

    void Update()
    {
        CalculateMovement();

        if (!GameManager.Instance.IsGameOver() && !_isDead && Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            EnemyShoot();
        }
    }

    void InitializeComponents()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("Animator is NULL!");

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Player is NULL!");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("AudioSource on Enemy is NULL!");

        if (_shields == null)
            Debug.LogError("Shields not found!");
        _shields.gameObject.SetActive(false);
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f && !_isBeingRelocated && !GameManager.Instance.IsGameOver())
            StartCoroutine(RelocateRoutine());
        else if (transform.position.y < -6f && GameManager.Instance.IsGameOver())
            transform.position = new Vector3(Random.Range(-9f, 9f), 6f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            if (_isShieldActive)
            {
                DeactivateShields();
                return;
            }
            DestroyEnemy();
        }
        else if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player == null)
                return;

            if (_isShieldActive)
            {
                DeactivateShields();
                player.Damage();
            }

            if (GameManager.Instance.IsGameOver())
                return;

            player.Damage();
            DestroyEnemy();
        }
    }

    IEnumerator RelocateRoutine()
    {
        _isBeingRelocated = true;
        Vector3 posToRelocate = new Vector3(Random.Range(-9f, 9f), Random.Range(4f, 5.5f), 0);
        yield return new WaitForSeconds(1f);
        GameObject particle = Instantiate(_spawnParticle, posToRelocate, Quaternion.identity);
        yield return new WaitForSeconds(1.8f);
        transform.position = posToRelocate;
        yield return new WaitForSeconds(5f);
        Destroy(particle);
        _isBeingRelocated = false;
    }

    void DestroyEnemy()
    {
        _anim.SetTrigger("OnEnemyDeath");
        GetComponent<BoxCollider2D>().enabled = false;
        _speed = 0;
        _player.IncreaseScore(_scoreAmount);
        _audioSource.Play();
        _isDead = true;
        Destroy(gameObject, 2.5f);
    }

    void EnemyShoot()
    {
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        foreach (var laser in lasers)
        {
            laser.AssignEnemyLaser();
        }
    }

    void SpawnWithShield()
    {
        if (Random.Range(0, 4) == 3)
            _shields.gameObject.SetActive(_isShieldActive = true);
    }

    void DeactivateShields()
    {
        _shields.gameObject.SetActive(_isShieldActive = false);
    }
}
