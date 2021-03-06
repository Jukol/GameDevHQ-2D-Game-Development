﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 4.0f;
    [SerializeField]
    protected Player _player;
    protected Animator _animator;
    protected AudioSource _audioSource;
    [SerializeField]
    protected GameObject _enemyLaserPrefab;
    protected float angle;
    protected bool _hit;
    protected Transform _target;
    [SerializeField]
    protected float minDistanceToPlayer = 2.0f;
    protected EnemyLaser _enemyLaser;
    protected float _powerupShootFireRate = 0.5f;
    protected bool _seenPowerup;
    protected float _timer = 0f;

    protected virtual void Start()
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

 
    protected virtual void Update()
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
    }

    protected virtual void Movement()
    {
        transform.position += -transform.up * _speed * Time.deltaTime;
        if (transform.position.y < -5.38f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 6.93f, 0);
        }
    }

    protected virtual void Ram()
    {
        float x = _target.position.x - transform.position.x;
        float y = _target.position.y - transform.position.y;
        float zRotation = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        StartCoroutine(Turn(zRotation + 90f, 0.5f));
        transform.position += -transform.up * 2.0f * Time.deltaTime;
        if (transform.position.y < -5.38f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 6.93f, 0);
        }
    }

    private IEnumerator Turn(float angle, float time)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(0, 0, angle);
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _hit = true;
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 2.5f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            _hit = true;
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 2.0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    protected virtual void FireLaser()
    {
        Instantiate(_enemyLaserPrefab, this.transform);
        _enemyLaserPrefab.transform.localPosition = new Vector3(-0.23f, -1.6f, 0);
        Instantiate(_enemyLaserPrefab, this.transform);
        _enemyLaserPrefab.transform.localPosition = new Vector3(0.23f, -1.6f, 0);
    }

    protected virtual IEnumerator FireLaserAtRandomTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
            if (_hit == false)
                FireLaser();
        }
    }



    ////// Smart Enemy
    ///Create an enemy type that knows when it’s behind the player, and fires a weapon backwards.

    //Enemy needs to cast a ray from tail. If the ray hits the player, the backward shooting is triggered.
    //Ray needs origin (Vector3) and direction (Vector3)
    //Origin is location of Enemy (transform.position)
    //Direction is opposite of Enemy's rotation:  (transform.up)

    public virtual bool BehindPlayer() //check if behind the player
    {
        return Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 20f, 1 << 9);
    }

    //Enemy pickups
    //Shoot ray. If crossed with powerup, shoot laser and destroy powerup

    protected virtual void ShootPowerup()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(-Vector2.up), 20f, 1 << 10);

        if (hit && !_seenPowerup)
        {
            FireLaser();
            _seenPowerup = true;
        }
        else if (hit)
        {
            _timer += Time.deltaTime;
            if (_timer >= _powerupShootFireRate)
            {
                FireLaser();
                _timer = 0f;
            }
        }
        else if (!hit)
        {
            _seenPowerup = false;
        }
    }

}
