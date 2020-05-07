using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _xDestroyDistance = 12f;
    [SerializeField]
    private float _yDestroyDistance = 8f;
    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else if(_isEnemyLaser == true)
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.Self);

        if (transform.position.y > _yDestroyDistance)
        {
            if (this.gameObject.transform.parent != null)
            {
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        if (transform.position.x > _xDestroyDistance || transform.position.x < -_xDestroyDistance)
        {
            if (this.gameObject.transform.parent != null)
            {
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -_yDestroyDistance)
        {
            if (this.gameObject.transform.parent != null)
            {
                Destroy(this.gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
                player.Damage();

            Destroy(this.gameObject);
        }
    }
}
