using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 7f;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] float _fireRate = 0.15f;
    Transform _laserContainer;
    float _canFire = -1f;

    void Start()
    {
        transform.position = new Vector3(0, -4f, 0);
        _laserContainer = GameObject.Find("LaserContainer").GetComponent<Transform>();
        if (_laserContainer == null)
            Debug.LogError("Laser Container is NULL!");
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);

        transform.Translate(direction * _playerSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 4.8f), 0f); ;

        if (transform.position.x > 11f || transform.position.x < -11f)
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        _canFire = Time.time + _fireRate;
        GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        laser.transform.SetParent(_laserContainer);
    }
}
