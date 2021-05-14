using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] int _powerUpId;
    [SerializeField] AudioClip _powerUpClip;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player _player = collision.GetComponent<Player>();
            if (_player == null)
                return;

            switch (_powerUpId)
            {
                case 0:
                    _player.ActivateTripleShot();
                    break;
                case 1:
                    _player.ActivateSpeed();
                    break;
                case 2:
                    _player.ActivateShields();
                    break;
                case 3:
                    _player.FillAmunition();
                    break;
                case 4:
                    _player.RestoreHealth();
                    break;
                case 5:
                    _player.DebuffPlayer();
                    break;
            }
            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);

            Destroy(gameObject);
        }
    }
}
