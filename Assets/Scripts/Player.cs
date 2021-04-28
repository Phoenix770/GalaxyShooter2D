using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 7f;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] float _fireRate = 0.15f;
    [SerializeField] int _maximumLives = 3;
    [SerializeField] float _tripleShotDuration = 5f;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] float _speedPowerUpDuration = 7f;
    GameManager _gameManager;
    Transform _laserContainer;
    float _canFire = -1f;
    int _currentLives;
    bool _isTripleShotActive;

    void Start()
    {
        transform.position = new Vector3(0, -4f, 0);
        _currentLives = _maximumLives;

        _laserContainer = GameObject.Find("LaserContainer").GetComponent<Transform>();
        if (_laserContainer == null)
            Debug.LogError("Laser Container is NULL!");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Game Manager is NULL!");
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
        GameObject laser;
        if (_isTripleShotActive)
            laser = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        laser.transform.SetParent(_laserContainer);
    }

    public void Damage()
    {
        _currentLives--;
        if (_currentLives < 1)
        {
            _gameManager.GameOver(true);
            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotPowerDownRoutine());
        _isTripleShotActive = true;
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _isTripleShotActive = false;
    }

    public void ActivateSpeed()
    {
        _playerSpeed *= 1.5f;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedPowerUpDuration);
        _playerSpeed /= 1.5f;
    }
}
