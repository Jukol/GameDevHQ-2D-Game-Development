using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //_audioSource = GetComponent<AudioSource>();

        //if (_audioSource == null)
        //{
        //    Debug.LogError("The Asteroid's Audio Source is NULL.");
        //}
        //else
        //{
        //    _audioSource.clip = _explosionSound;
        //}
        //_audioSource.Play();
        Destroy(this.gameObject, 2.5f);
    }

}
