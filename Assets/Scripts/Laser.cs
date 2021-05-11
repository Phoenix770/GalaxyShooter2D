using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _speed = 9f;

    bool _isEnemyLaser = false;
    float _verticalLimitNegative = -6f;
    float _verticalLimitPositive = 7f;

    void Update()
    {
        if (!_isEnemyLaser)
            MoveUp();
        else
            MoveDown();
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > _verticalLimitPositive)
        {
            if (transform.parent.name == "TripleLaserShot(Clone)")
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _verticalLimitNegative)
        {
            if (transform.parent.name == "TripleLaserShot(Clone)")
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gameObject.tag == "EnemyWeapon")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                player.Damage();

            Destroy(transform.parent.gameObject);
        }
    }
}
