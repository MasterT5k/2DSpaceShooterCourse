using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _horizontalLimits = 8;
    [SerializeField]
    private float _verticalLimits = -8f;
    [SerializeField]
    private int _points = 10;
    [SerializeField]
    private GameObject _thrusters;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Transform _laserSpawn;
    [SerializeField]
    private float _minFireRate = 3f;
    [SerializeField]
    private float _maxFireRate = 7f;
    private float _canFire = -1;

    private Player _player;
    private AudioManager _audioManager;
    private Animator _animator;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        _animator = GetComponent<Animator>();
        
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is NULL");
        }

        if (_animator == null)
        {
            Debug.LogError("Needs Animator!");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float randomX = Random.Range(-_horizontalLimits, _horizontalLimits);

        if (transform.position.y < _verticalLimits)
        {
            transform.position = new Vector3(randomX, -_verticalLimits, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.ScoreChange(_points/2);
                player.Damage();
            }

            _animator.SetTrigger("OnEnemyDeath");

            Destroy(GetComponent<Collider2D>());
            Destroy(_thrusters, 0.1f);
            Destroy(this.gameObject, 2.3f);
        }

        if (other.tag == "Laser")
        {
            _player.ScoreChange(_points);
            _animator.SetTrigger("OnEnemyDeath");

            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.3f);
        }
    }
    void FireLaser()
    {
        float fireRate = Random.Range(_minFireRate, _maxFireRate);
        _canFire = Time.time + fireRate;

        _audioManager.LaserSound();
        
        GameObject enemyLaser = Instantiate(_laserPrefab, _laserSpawn.position, Quaternion.identity);

        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }
}
