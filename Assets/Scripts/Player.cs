using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;
    private float _speedMultiplier = 3.5f;
    private float _shiftSpeedMultiplier = 3.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _multiShotPrefab;
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _tripleShotActive;
    private bool _multiShotActive;
    private bool _shieldActive;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    private int _shieldStrength = 3;
    [SerializeField]
    private int _ammo;
    [SerializeField]
    private int _maxAmmo;
    public bool flickerStarted;
    [SerializeField]
    private float _thrusterCoolDownTimer = 10.0f;
    [SerializeField]
    private GameObject _bar;
    private float _barValue;
    [SerializeField]
    private GameObject _camera;

    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _barValue = _bar.transform.localScale.x;
        _ammo = _maxAmmo;
    
        
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        _shield.SetActive(false);

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _uiManager.UpdateAmmo(_ammo, _maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            FireLaser();
            _ammo--;
            _uiManager.UpdateAmmo(_ammo, _maxAmmo);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _ammo <= 0 && !flickerStarted)
        {
            flickerStarted = true;
            _uiManager.OutOfAmmoFlicker();
        }
            
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        if (Input.GetKey(KeyCode.LeftShift) && _thrusterCoolDownTimer > 0)
        {
            _thrusterCoolDownTimer -= Time.deltaTime;
            transform.Translate(direction * _speed * _shiftSpeedMultiplier * Time.deltaTime);
            ThrustBarUpdate();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(ThrustTimerRoutine());
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

        IEnumerator ThrustTimerRoutine()
        {
            yield return new WaitForSeconds(5.0f);
            while (_thrusterCoolDownTimer < 10.0f)
            {
                
                _thrusterCoolDownTimer += Time.deltaTime;
                ThrustBarUpdate();
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
        }

        
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_multiShotActive)
        {
            Instantiate(_multiShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        _camera.GetComponent<CameraShake>().ShakeCamera();
        
        if (_shieldActive && _shieldStrength == 3)
        {
            _shield.GetComponent<SpriteRenderer>().material.color = Color.yellow;
            _shieldStrength--;
            return;
        }
        else if (_shieldActive && _shieldStrength == 2)
        {
            _shield.GetComponent<SpriteRenderer>().material.color = Color.red;
            _shieldStrength--;
            return;
        }
        else if (_shieldActive && _shieldStrength == 1)
        {
            _shield.GetComponent<SpriteRenderer>().material.color = Color.clear;
            _shieldStrength--;
            return;
        }
        else if (_shieldActive && _shieldStrength == 0)
        {
            _shieldActive = false;
            _shield.SetActive(false);
        }


        if (!_shieldActive)
        {
            _lives--;
        }
        

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        } else if (_lives == 1) 
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOverSequence();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedUpActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedUpActivePowerDownRoutine());
    }

    IEnumerator SpeedUpActivePowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldStrength = 3;
        _shield.SetActive(true);
        _shield.GetComponent<SpriteRenderer>().material.color = Color.white;
    }

    public void AmmoActive()
    {
        _ammo += 5;
        if (_ammo > _maxAmmo)
        {
            _ammo = _maxAmmo;
        }
        _uiManager.UpdateAmmo(_ammo, _maxAmmo);
    }

    public void HealthActive()
    {
        _lives++;
        if (_lives > 3)
        {
            _lives = 3;
        }
        _uiManager.UpdateLives(_lives);
        if (_rightEngine.activeSelf == true)
        {
            _rightEngine.SetActive(false);
        }
        else if (_leftEngine.activeSelf == true)
        {
            _leftEngine.SetActive(false);
        }
    }

    public void MultiShotActive()
    {
        _multiShotActive = true;
        StartCoroutine(MultiShotPowerDownRoutine());
    }

    IEnumerator MultiShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _multiShotActive = false;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void ThrustBarUpdate()
    {
        _barValue = _thrusterCoolDownTimer / 10;
        _bar.transform.localScale = new Vector3(_barValue, _bar.transform.localScale.y, _bar.transform.localScale.z);
    }

}
