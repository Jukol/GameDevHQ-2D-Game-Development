using System.Collections;
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
            Debug.Log("Enemy close");
            Ram();
        }
        else
            Movement();
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
}
