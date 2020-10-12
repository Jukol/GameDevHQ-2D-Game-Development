using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField]
    protected GameObject _enemyLaserPrefab;

    [SerializeField]
    private GameObject _explosionContainer;

    Vector3 nextTarget;
    float speed;
    float waitTime;
    float timer;

    bool firingStarted;

    [SerializeField]
    int damage;
    
    [SerializeField]
    int health;

    bool destroyed;

    BossDyingExplosions _dyingExplosions;

    private void Start()
    {
        damage = health;
        _explosionContainer.SetActive(false);
        
        nextTarget = new Vector3(0, 1.83f, 0);

        _audioSource = GetComponent<AudioSource>();

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("The Animator is NULL.");
        }

        speed = 1f;
        nextTarget = new Vector3(0, 1.5f, 0);
        waitTime = 3.0f;
        timer = 3.0f;
        _dyingExplosions = GetComponentInChildren<BossDyingExplosions>();

        _enemyLaserPrefab.GetComponent<AudioSource>().volume = 0.2f;
    }

    private void Update()
    {
        Movement();
        
    }

    void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTarget, speed * Time.deltaTime);
        if (transform.position == nextTarget)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (!firingStarted)
                {
                    StartCoroutine(FireLaserRoutine());
                }
                float x = Random.Range(-7.78f, 7.78f);
                float y = Random.Range(1.83f, 5.19f);
                nextTarget = new Vector3(x, y, 0);
                timer = waitTime;
            }
        }
    }

    private void FireLaser()
    {
        if (!destroyed)
        {
            Instantiate(_enemyLaserPrefab, this.transform);
            _enemyLaserPrefab.transform.localPosition = new Vector3(-1.8f, -1.14f, 0);
            Instantiate(_enemyLaserPrefab, this.transform);
            _enemyLaserPrefab.transform.localPosition = new Vector3(-0.9f, -1.87f, 0);
            Instantiate(_enemyLaserPrefab, this.transform);
            _enemyLaserPrefab.transform.localPosition = new Vector3(0, -1.059f, 0);
            Instantiate(_enemyLaserPrefab, this.transform);
            _enemyLaserPrefab.transform.localPosition = new Vector3(1.8f, -1.14f, 0);
            Instantiate(_enemyLaserPrefab, this.transform);
            _enemyLaserPrefab.transform.localPosition = new Vector3(0.9f, -1.87f, 0);
        }
        
    }

    private IEnumerator FireLaserRoutine()
    {
        //Make a timer to shoot for 10s with 5s breaks
        firingStarted = true;
        while (true)
        {
            float currentTime = Time.time;
            float timer = 10f;
            while ((Time.time - currentTime) <= timer)
            {
                FireLaser();
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            
            
            if (Damage() <= 0)
            {
                destroyed = true;
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                Destroy(this.gameObject, 2.8f);
            }
            
            
        }

        if (other.tag == "Laser")
        {
            
            if (_player != null)
            {
                _player.AddScore(10);
            }

            if (Damage() <= health * 0.5f) //turn on explosions on body, damage 50% of health
            {
                _explosionContainer.SetActive(true);
            }
            else if (Damage() <= health * 0.3f)
            {
                _dyingExplosions.Seconds(10f);
            }
            else if (Damage() <= health * 0.1f)
            {
                _dyingExplosions.Seconds(1f);
            }
            
            if (Damage() <= 0)
            {
                destroyed = true;
                _animator.SetTrigger("OnEnemyDeath");
                //_speed = 2.0f;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }

            
        }
    }

    private int Damage()
    {
        damage--;
        return damage;
    }

}
