using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is NULL");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            Destroy(other.gameObject);

            _spawnManager.StartSpawnRoutines();
            _audioManager.ExplosionSound();

            Destroy(this.gameObject, 0.25f);
        }
    }
}
