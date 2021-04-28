using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;
    [SerializeField] int _scoreAmount = 10;
    Player _player;
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("Animator is NULL!");

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Player is NULL!");
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
            transform.position = new Vector3(Random.Range(-9f, 9f), 7f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            DestroyEnemy();
        }

        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player == null)
                return;

            player.Damage();
            DestroyEnemy();
        }
    }

    void DestroyEnemy()
    {
        _anim.SetTrigger("OnEnemyDeath");
        GetComponent<BoxCollider2D>().enabled = false;
        _speed = 0;
        _player.IncreaseScore(_scoreAmount);
        Destroy(gameObject, 2.5f);
    }
}
