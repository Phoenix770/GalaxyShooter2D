using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class Player : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float _playerSpeed = 7f;
    bool _thrustersActivated = false;
    int _thrusterValue;
    int _maxThrusterValue = 250;
    bool _overheated = false;
    [Tooltip("Original Color of the Thruster Bar")] Color _originalColor;

    [Header("Shooting")]
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] float _tripleShotDuration = 5f;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] float _fireRate = 0.15f;

    [SerializeField] int _maximumAmmo = 15;
    float _canFire = -1f;
    bool _isTripleShotActive;
    int _currentAmmo;

    [Header("PowerUps")]
    [SerializeField] float _speedPowerUpDuration = 7f;

    [Header("GameObjects")]
    [SerializeField] GameObject _rightEngine;
    [SerializeField] GameObject _leftEngine;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] AudioClip _laserSoundClip;
    [SerializeField] AudioClip _noAmmoClip;
    [SerializeField] Thrusters _thrusterBar;

    [Header("CameraShake")]
    [SerializeField] float _shakeDuration = 0.3f;
    [SerializeField] float _shakeMagnitude = 0.4f;

    [Header("Lives & Score")]
    [SerializeField] int _maximumLives = 3;
    int _currentLives;
    int _score = 0;

    [Header("Components")]
    GameManager _gameManager;
    Transform _laserContainer;
    ShieldBehavior _shields;
    UIManager _uiManager;
    AudioSource _audioSource;
    AudioSource _noAmmo;
    CameraShake _cameraShake;
    CanvasRenderer _thrusterRenderer;
    Transform _thrusters;

    #endregion

    void Start()
    {
        InitializeComponents();

        InitializeValues();

        transform.position = new Vector3(0, -4f, 0);
    }

    void Update()
    {
        PlayerMovement();

        FireWeapon();

        ActivateThrusters();
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);

        transform.Translate(direction * _playerSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 4.8f), 0f); ;

        if (transform.position.x > 11f || transform.position.x < -11f)
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
    }

    void FireWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_currentAmmo == 0)
                _noAmmo.Play();
            else
                Fire();
        }
    }

    void ActivateThrusters()
    {
        if (!GameManager.Instance.IsGameOver() && Input.GetKeyDown(KeyCode.LeftShift) && !_thrustersActivated && !_overheated)
        {
            StartCoroutine(ActivateThrustersRoutine());
        }
    }

    void InitializeComponents()
    {
        _laserContainer = GameObject.Find("LaserContainer").GetComponent<Transform>();
        if (_laserContainer == null)
            Debug.LogError("Laser Container is NULL!");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Game Manager is NULL!");

        _shields = gameObject.GetComponentInChildren<ShieldBehavior>();
        if (_shields == null)
            Debug.LogError("Shields not found!");
        _shields.gameObject.SetActive(false);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("UI Manager is NULL!");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("AudioSource on the Player is NULL!");
        else
            _audioSource.clip = _laserSoundClip;

        _noAmmo = GameObject.Find("NoAmmo").GetComponent<AudioSource>();
        if (_noAmmo == null)
            Debug.LogError("AudioSource NoAmmo is NULL!");

        _thrusterBar = GameObject.Find("Thruster_HUD").GetComponent<Thrusters>();
        if (_thrusterBar == null)
            Debug.LogError("Thruster HUD is NULL!");
        _thrusterBar.SetValue(_maxThrusterValue);
        _thrusterValue = _maxThrusterValue;

        _thrusterRenderer = _thrusterBar.gameObject.transform.GetChild(0).GetComponent<CanvasRenderer>();
        if (_thrusterRenderer == null)
            Debug.LogError("Thruster Renderer is NULL!");
        _thrusters = gameObject.transform.GetChild(1); //Thruster
        _originalColor = _thrusterRenderer.GetColor();

        _cameraShake = GameObject.Find("CameraShake").GetComponent<CameraShake>();
        if (_cameraShake == null)
            Debug.LogError("CameraShake is NULL!");
    }

    void InitializeValues()
    {
        _currentLives = _maximumLives;
        _rightEngine.gameObject.SetActive(false);
        _leftEngine.gameObject.SetActive(false);
        _currentAmmo = _maximumAmmo;
    }

    void Fire()
    {
        _canFire = Time.time + _fireRate;
        GameObject laser;
        if (_isTripleShotActive)
            laser = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        _currentAmmo--;
        _uiManager.UpdateAmmo(_currentAmmo, _maximumAmmo);
        laser.transform.SetParent(_laserContainer);
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_shields.AreShieldsActive())
        {
            _shields.DamageShields();
            return;
        }

        _currentLives--;
        StartCoroutine(_cameraShake.Shake(_shakeDuration, _shakeMagnitude));
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

    #region PowerUps
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
        _shields.ActivateShields();
    }

    public void FillAmunition()
    {
        _uiManager.UpdateAmmo(_currentAmmo = _maximumAmmo, _maximumAmmo);
    }

    public void RestoreHealth()
    {
        switch (++_currentLives)
        {
            case 4:
                _currentLives--;
                break;
            case 3:
                _rightEngine.gameObject.SetActive(false);
                break;
            case 2:
                _leftEngine.gameObject.SetActive(false);
                break;
        }
        _uiManager.UpdateLives(_currentLives);
    }
    #endregion

    public void IncreaseScore(int scoreAmount)
    {
        _score += scoreAmount;
        _uiManager.UpdateScore(_score);
    }

    public void DestroyPlayer()
    {
        StartCoroutine(_cameraShake.Shake(_shakeDuration, _shakeMagnitude));
        _gameManager.GameOver(true);
        _uiManager.UpdateLives(_currentLives = 0);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _playerSpeed = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.position = new Vector3(-100f, -100f, -100f);
        Destroy(gameObject, 4.25f);
    }

    #region Thrusters

    IEnumerator ActivateThrustersRoutine()
    {
        _thrustersActivated = true;
        _playerSpeed *= 2f;
        _thrusters.localScale = new Vector3(1.5f, 1.5f, 1f);
        _thrusters.position = new Vector3(transform.position.x, transform.position.y - 2f, 0f);
        while (Input.GetKey(KeyCode.LeftShift) && _thrusterValue > 0)
        {
            yield return new WaitForSeconds(0.01f);
            _thrusterBar.SetValue(--_thrusterValue);
        }
        _thrusters.localScale = new Vector3(1f, 1f, 1f);
        _thrusters.position = new Vector3(transform.position.x, transform.position.y - 1.6f, 0f);
        _thrustersActivated = false;
        StartCoroutine(ThrusterFillUpRoutine());
        yield return new WaitForSeconds(0.25f);
        _playerSpeed /= 2f;
    }

    IEnumerator ThrusterFillUpRoutine()
    {
        if (_thrusterValue == 0)
        {
            _thrusterBar.Overheated(_overheated = true);
            _thrusterRenderer.SetColor(new Color(255f, 0f, 0f));
        }

        if (_overheated)
        {
            while (_thrusterValue != _maxThrusterValue)
            {
                _thrusterBar.SetValue(++_thrusterValue);
                yield return new WaitForSeconds(0.01f);
            }
            _thrusterRenderer.SetColor(_originalColor);
            _thrusterBar.Overheated(_overheated = false);
        }
        else
        {
            while (_thrusterValue != _maxThrusterValue && !_thrustersActivated)
            {
                _thrusterBar.SetValue(++_thrusterValue);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    #endregion
}