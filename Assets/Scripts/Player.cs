using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float _speed = 5f;
    private float _baseSpeed;
    [SerializeField]
    private Transform _engineTrail;
    private Vector3 _idleTrailSize;
    private Vector3 _fullTrailSize;

    [SerializeField]
    private float _thrusterBoost = 2f;
    [SerializeField]
    private float _thrusterBurnLength = 5;
    private float _elapsedTime = 0;
    private bool _isThrusterActive = false;
    private bool _canThruster = true;

    [SerializeField]
    private float _horizontalBounds = 11.3f;
    [SerializeField]
    private float _bottomBounds = -4.8f;

    [Header("Weapon Settings")]
    [SerializeField]
    private Transform _laserSpawn;
    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    private int _maxAmmoCount = 15;
    private int _ammoCount;

    [Header("PowerUp Settings")]
    [SerializeField]
    private GameObject _shieldVisualizer;
    private bool _isShieldActive = false;
    private int _shieldHits = 0;
    private SpriteRenderer _shieldGraphic;

    [SerializeField]
    private float _speedBoostMultiplier = 2f;
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private GameObject _spreadShotPrefab;
    private bool _isSpreadShotActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isTripleShotActive = false;

    [SerializeField]
    private float _powerUpDuration = 5f;

    [Header("Health Settings")]
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject[] _fires;
    private int _lives = 3;

    private int _score = 0;
    private Animator _anim;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        _fullTrailSize = _engineTrail.transform.localScale;
        _idleTrailSize = _fullTrailSize / 2;

        _shieldGraphic = _shieldVisualizer.GetComponent<SpriteRenderer>();
        _baseSpeed = _speed;
        _ammoCount = _maxAmmoCount;

        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (_anim == null)
            Debug.LogError("Animator is NULL");

        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is NULL.");

        if (_uiManager == null)
            Debug.LogError("UI Manager is NULL");

        if (_audioManager == null)
            Debug.LogError("Audio Manager is NULL");

        _uiManager.ChangeScoreText(_score);
        _uiManager.ChangeAmmo(_ammoCount);
        _uiManager.UpdateThrusterBar(_elapsedTime, _thrusterBurnLength);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            FireLaser();
    }

    void CalculateMovement()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canThruster == true && _elapsedTime < _thrusterBurnLength && _isSpeedBoostActive == false)
        {
            _speed += _thrusterBoost;
            _isThrusterActive = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _baseSpeed;
            _isThrusterActive = false;
            _canThruster = false;
        }

        if (_isThrusterActive == true)
        {
            if (_elapsedTime > _thrusterBurnLength)
            {
                _elapsedTime = _thrusterBurnLength;
                _canThruster = false;
                _speed = _baseSpeed;
                return;
            }

            _elapsedTime += Time.deltaTime;
            _uiManager.UpdateThrusterBar(_elapsedTime, _thrusterBurnLength);
        }
        else if (_isThrusterActive == false && _elapsedTime > 0)
        {
            _elapsedTime -= Time.deltaTime;
            _uiManager.UpdateThrusterBar(_elapsedTime, _thrusterBurnLength);

            if (_elapsedTime < 0)
            {
                _elapsedTime = 0;
                _canThruster = true;
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        _anim.SetFloat("Direction", horizontalInput);

        if (direction.x != 0 || direction.y != 0)
        {
            _engineTrail.localScale = _fullTrailSize;
        }
        else
        {
            _engineTrail.localScale = _idleTrailSize;
        }

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.x > _horizontalBounds)
        {
            transform.position = new Vector3(-_horizontalBounds, transform.position.y, 0);
        }
        else if (transform.position.x < -_horizontalBounds)
        {
            transform.position = new Vector3(_horizontalBounds, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _bottomBounds, 0), 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isSpreadShotActive == true)
        {
            Instantiate(_spreadShotPrefab, _laserSpawn.position, Quaternion.identity);
        }
        else if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, _laserSpawn.position, Quaternion.identity);
        }
        else if (_ammoCount > 0)
        {
            _ammoCount--;
            _uiManager.ChangeAmmo(_ammoCount);
            Instantiate(_laserPrefab, _laserSpawn.position, Quaternion.identity);
        }
        else if (_ammoCount < 1)
        {
            _ammoCount = 0;
            return;
        }

        _audioManager.LaserSound();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldHits++;

            switch (_shieldHits)
            {
                case 1:
                    _shieldGraphic.color = new Color(0.9f, 0.4f, 1f, 1f);
                    break;
                case 2:
                    _shieldGraphic.color = new Color(1f, 0.175f, 0.325f, 1f);
                    break;
                case 3:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
                default:
                    break;
            }
            return;
        }

        _lives--;
        LivesChange();
        Camera.main.GetComponent<CameraShake>().ShakeCamera();

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _audioManager.ExplosionSound();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
        else if (_lives < 3)
        {
            _fires[0].gameObject.SetActive(true);

            if (_lives < 2)
                _fires[1].gameObject.SetActive(true);
        }
    }

    public void SpreadShotActive()
    {
        _isSpreadShotActive = true;
        _audioManager.PowerUPSound();

        StartCoroutine(SpreadShotPowerDownRoutine());
    }

    IEnumerator SpreadShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpDuration);
        _isSpreadShotActive = false;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _audioManager.PowerUPSound();

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpDuration);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        _audioManager.PowerUPSound();

        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpDuration);
        _isSpeedBoostActive = false;
        _speed /= _speedBoostMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldHits = 0;
        _shieldGraphic.color = new Color(0f, 0.75f, 1f, 1f);
        _audioManager.PowerUPSound();

    }

    public void AmmoPickup()
    {
        _ammoCount = _maxAmmoCount;
        _uiManager.ChangeAmmo(_ammoCount);
    }

    public void ScoreChange(int points = 0)
    {
        _score += points;
        _uiManager.ChangeScoreText(_score);
        _audioManager.ExplosionSound();
    }

    public void LivesChange(int healthAmount = 0)
    {
        _lives += healthAmount;

        if (_lives > 3)
        {
            _lives = 3;
        }
        else if (_lives > 1)
        {
            _fires[1].gameObject.SetActive(false);

            if (_lives > 2)
                _fires[0].gameObject.SetActive(false);
        }
        _uiManager.ChangeLives(_lives);
    }
}
