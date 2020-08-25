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

    // Update is called once per frame

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }
    void Update()
    {
        transform.position += -transform.up * _speed * Time.deltaTime;
        if (transform.position.y < -8f)
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
