using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{
    Vector3 newPositionToRight;
    Vector3 newPositionToLeft;

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
        angle = Random.Range(-30f, 30f);
        transform.Rotate(0, 0, angle);

        
    }

    protected override void Update()
    {

        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance <= minDistanceToPlayer)
        {
            Ram();
        }
        else
            Movement();

        BehindPlayer();
        ShootPowerup();

        newPositionToRight = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        newPositionToLeft = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
    }


    public void DetectPlayersLaser(float laserX)
    {
        Debug.Log("Laser coming.");
        if (transform.position.x >= laserX)
            StartCoroutine(SmoothMove(newPositionToRight, 0.5f));
        else
            StartCoroutine(SmoothMove(newPositionToLeft, 0.5f));
    }

    private IEnumerator SmoothMove(Vector3 toPositionX, float time)
    {
        Vector3 fromPosition = transform.position;
        Vector3 toPosition = toPositionX;
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
             transform.position = Vector3.Lerp(fromPosition, toPosition, t);
            yield return null;
        }
    }
}
