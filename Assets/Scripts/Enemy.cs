using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;

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
            Destroy(gameObject);
        }

        if (collision.tag == "Player")
        {
            Player _player = collision.GetComponent<Player>();
            if (_player == null)
                return;
            _player.Damage();
            Destroy(gameObject);
        }
    }
}
