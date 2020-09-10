using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private Player _player;
    private Vector3 _laserDirection;
    private float _laserDisappearPositionByY;
    private Enemy _parentEnemy;

    // Update is called once per frame

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
       
        _laserDisappearPositionByY = -8f;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _parentEnemy = transform.parent.gameObject.GetComponent<Enemy>();

        if (_parentEnemy.BehindPlayer())// shoot back if behind the player
            _laserDirection = transform.up;
        else
            _laserDirection = -transform.up;
    }
    void Update()
    {
        transform.position += _laserDirection * _speed * Time.deltaTime;
        if (transform.position.y < _laserDisappearPositionByY)
        {
 
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
        }
    }

    public void ChangeLaserDirection()
    {
        _laserDirection = -_laserDirection;
        _laserDisappearPositionByY = -_laserDisappearPositionByY;
    }
}
