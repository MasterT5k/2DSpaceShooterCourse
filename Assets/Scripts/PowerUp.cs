using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private float _offScreenDistance = -8f;

    private enum PowerUpType
    {
        tripleShot,
        speed,
        shield,
        ammo,
        health,
        spreadShot
    }

    [SerializeField]
    private PowerUpType _powerUpType = PowerUpType.tripleShot;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _offScreenDistance)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            switch (_powerUpType)
            {
                case PowerUpType.tripleShot:
                    player.TripleShotActive();
                    break;
                case PowerUpType.speed:
                    player.SpeedBoostActive();
                    break;
                case PowerUpType.shield:
                    player.ShieldActive();
                    break;
                case PowerUpType.ammo:
                    player.AmmoPickup();
                    break;
                case PowerUpType.health:
                    player.LivesChange(1);
                    break;
                case PowerUpType.spreadShot:
                    player.SpreadShotActive();
                    break;
                default:
                    Debug.Log("Default Value");
                    break;
            }
            
            Destroy(this.gameObject);
        }
    }
}
