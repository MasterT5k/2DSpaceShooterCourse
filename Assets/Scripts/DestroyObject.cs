using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]
    private float _destroyDelay = 3f;

    void Start()
    {
        Destroy(this.gameObject, _destroyDelay);
    }
}
