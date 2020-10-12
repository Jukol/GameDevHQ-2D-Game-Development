using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private Player _player;
    private float _laserDisappearPositionByY;

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

    }
    void Update()
    {
        transform.position += -transform.up * _speed * Time.deltaTime;
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

}
