using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : Enemy
{
    [SerializeField]
    private GameObject _shield;
    private bool _shieldActive = true;
    protected override void Movement()
    {
        transform.position += -transform.up * _speed * Time.deltaTime;
        if (transform.position.y < -5.38f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 6.93f, 0);
        }
    }


    protected override void FireLaser()
    {
        Instantiate(_enemyLaserPrefab, this.transform);
        _enemyLaserPrefab.transform.localPosition = new Vector3(-0.33f, -1.35f, 0);
        Instantiate(_enemyLaserPrefab, this.transform);
        _enemyLaserPrefab.transform.localPosition = new Vector3(0.23f, -1.35f, 0);
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

    protected override void OnTriggerEnter2D(Collider2D other)
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
            if (_shieldActive == true)
            {
                _shield.SetActive(false);
                _shieldActive = false;
            }
            else
            {
                Destroy(this.gameObject, 2.8f);
            }
            
        }

        if (other.tag == "Laser")
        {
            _hit = true;
            Destroy(other.gameObject);
            if (_shieldActive == true)
            {
                _shield.SetActive(false);
                _shieldActive = false;
            }
            else
            {
                _animator.SetTrigger("OnEnemyDeath");
                _speed = 2.0f;
                _audioSource.Play();
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
            
        }
    }

}
