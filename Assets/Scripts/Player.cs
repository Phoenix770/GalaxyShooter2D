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
    [SerializeField] GameObject _rightEngine;
    [SerializeField] GameObject _leftEngine;
    [SerializeField] GameObject _explosionPrefab;

    GameManager _gameManager;
    Transform _laserContainer;
    GameObject _shields;
    float _canFire = -1f;
    int _currentLives;
    bool _isTripleShotActive;
    bool _isShieldsActive;
    int _score = 0;
    UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, -4f, 0);
        _currentLives = _maximumLives;
        _rightEngine.gameObject.SetActive(false);
        _leftEngine.gameObject.SetActive(false);

        _laserContainer = GameObject.Find("LaserContainer").GetComponent<Transform>();
        if (_laserContainer == null)
            Debug.LogError("Laser Container is NULL!");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Game Manager is NULL!");

        _shields = GameObject.Find("ShieldsEffect");
        if (_shields == null)
            Debug.LogError("Shields not found!");
        _shields.SetActive(false);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("UI Manager is NULL!");
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
        if (_isShieldsActive)
        {
            _isShieldsActive = false;
            _shields.SetActive(false);

            return;
        }

        _currentLives--;
        _uiManager.UpdateLives(_currentLives);

        if (_currentLives == 2)
            _rightEngine.gameObject.SetActive(true);
        else if (_currentLives == 1)
            _leftEngine.gameObject.SetActive(true);

        if (_currentLives < 1)
        {
            DestroyPlayer();
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

    public void ActivateShields()
    {
        _isShieldsActive = true;
        _shields.SetActive(true);
    }

    public void IncreaseScore(int scoreAmount)
    {
        _score += scoreAmount;
        _uiManager.UpdateScore(_score);
    }

    public void DestroyPlayer()
    {
        _gameManager.GameOver(true);
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _playerSpeed = 0;
        Destroy(gameObject, 0.25f);
        Destroy(explosion, 3f);
    }

}
