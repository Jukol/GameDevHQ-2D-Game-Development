using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDyingExplosions : MonoBehaviour
{
    Animator _myAnimator;
    float _seconds;
    float _receivedSeconds;

    void OnEnable()
    {
        _myAnimator = GetComponent<Animator>();
        StartCoroutine(ExplosionRoutine());
        _receivedSeconds = 30f;
    }

    IEnumerator ExplosionRoutine()
    {
        while (true)
        {
            _seconds = Random.Range(1f, _receivedSeconds);
            yield return new WaitForSeconds(_seconds);
            _myAnimator.SetTrigger("OnHit");
        }
    }

    public float Seconds(float seconds)
    {
        _receivedSeconds = seconds;
        return _receivedSeconds;
    }
}
