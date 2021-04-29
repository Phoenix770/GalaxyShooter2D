using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] int _scoreAmount = 10;
    [SerializeField] GameObject _spawnParticle;
    Player _player;
    Animator _anim;
    AudioSource _audioSource;
    bool _isBeingRelocated;

    private void Start()
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
    }

    void Update()
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
            DestroyEnemy();
        }
        else if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player == null)
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
        Destroy(gameObject, 2.5f);
    }
}
