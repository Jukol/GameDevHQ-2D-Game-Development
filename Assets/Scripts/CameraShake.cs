using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _shakeDirection;
    private Vector3 _originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
        _shakeDirection.x = Random.Range(-0.2f, 0.2f);
        _shakeDirection.y = Random.Range(0.8f, 1.2f);
        _shakeDirection.z = -10.0f;
    }

    public void ShakeCamera()
    {

       StartCoroutine(CameraShakeRoutine());
    }

    IEnumerator CameraShakeRoutine()
    {

        for (int i = 0; i < 5; i++)
        {
            transform.position = _shakeDirection;
            yield return new WaitForSeconds(0.05f);
            transform.position = _originalPosition;
            yield return new WaitForSeconds(0.05f);
        }


    }
}
