using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[SerializeField]
    //private float _speed = 1f;
    [SerializeField]
    private float _rotationSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    [SerializeField]
    
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
     
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.5f);
        }
    }
}
