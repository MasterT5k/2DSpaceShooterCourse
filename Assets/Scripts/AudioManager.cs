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
    [SerializeField]
    private AudioClip _backgroundMusic;
    [SerializeField]
    private AudioSource _audioSourceSFX;
    [SerializeField]
    private AudioSource _audioSourceBG;

    void Start()
    {
        _audioSourceBG.clip = _backgroundMusic;
        _audioSourceBG.Play();
    }

    public void LaserSound()
    {
        _audioSourceSFX.PlayOneShot(_laserSound);
    }

    public void PowerUPSound()
    {
        _audioSourceSFX.PlayOneShot(_powerUPSound);
    }

    public void ExplosionSound()
    {
        _audioSourceSFX.PlayOneShot(_explosionSound);
    }
}
