using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _horizontalBounds = 9f;
    [SerializeField]
    private float _bottomBounds = -3.8f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private Transform _laserSpawn;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject[] _fires;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

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

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, _laserSpawn.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, _laserSpawn.position, Quaternion.identity);
        }

        _audioManager.LaserSound();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        LivesChange();

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
            {
                _fires[1].gameObject.SetActive(true);
            }
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _audioManager.PowerUPSound();

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        _audioManager.PowerUPSound();

        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _audioManager.PowerUPSound();

    }

    public void ScoreChange(int points = 0)
    {
        _score += points;
        _uiManager.ChangeScoreText(_score);
        _audioManager.ExplosionSound();
    }

    public void LivesChange()
    {
        _uiManager.ChangeLives(_lives);
    }
}
