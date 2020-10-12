using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 8.0f;

    private Animator _animator;

    private AudioSource _audioSource;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("The Animator is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            _speed = 0;
            transform.parent = collision.transform;
            _audioSource.Play();
            _animator.SetTrigger("OnHit");
            Destroy(this.gameObject, 3f);
        }
    }
        
}
