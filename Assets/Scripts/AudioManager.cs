using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _powerUPSound;
    [SerializeField]
    private AudioClip _explosionSound;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Needs an Audio Source");
        }
    }

    public void LaserSound()
    {
        _audioSource.PlayOneShot(_laserSound);
    }

    public void PowerUPSound()
    {
        _audioSource.PlayOneShot(_powerUPSound);
    }

    public void ExplosionSound()
    {
        _audioSource.PlayOneShot(_explosionSound);
    }
}
