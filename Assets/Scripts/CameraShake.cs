using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _magnitude = 0.2f;
    [SerializeField]
    private float _duration = 0.15f;
    private bool _isRunning = false;

    public void ShakeCamera()
    {
        if (_isRunning == false)
            StartCoroutine(Shake(_duration, _magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        _isRunning = true;

        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        _isRunning = false;
    }
}
