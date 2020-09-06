using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy3 : Enemy
{
    float _rotateSpeed = 2.0f;
    
    protected override void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("The Animator is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        _target = _player.GetComponent<Transform>();

        StartCoroutine(FireLaserAtRandomTime());
        angle = 30f;
        StartCoroutine(ZigZagRoutine());
    }

    
    private IEnumerator Turn(float angle, float time)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(0, 0, angle);
        for(var t = 0f; t < 1; t += Time.deltaTime/time)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    private IEnumerator ZigZagRoutine()
    {
        while (true)
        {
            StartCoroutine(Turn(angle, 1.0f));
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(Turn(-angle, 1.0f));
            yield return new WaitForSeconds(2.0f);
        }
    }

    protected override void FireLaser()
    {
        Instantiate(_enemyLaserPrefab, this.transform);
        _enemyLaserPrefab.transform.localPosition = new Vector3(0, -1.25f, 0);
    }

    protected override IEnumerator FireLaserAtRandomTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (_hit == false)
                FireLaser();
        }
    }


}
