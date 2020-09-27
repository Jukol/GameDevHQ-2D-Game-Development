using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID; //0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Ammo_Powerup, 4 = Health, 5 - Multishot
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private Player _player;
    private float _speedToPlayer = 6.0f;
    bool buttonPushed = false;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C) && buttonPushed == false)
        {
            RushToPlayer();
            buttonPushed = true;
        }
        else if (buttonPushed == true)
        {
            RushToPlayer();
        }

        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y < -5.8f)
            {
                Destroy(this.gameObject);
            }
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedUpActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoActive();
                        break;
                    case 4:
                        player.HealthActive();
                        break;
                    case 5:
                        player.MultiShotActive();
                        break;
                    case 6:
                        player.Damage();
                        break;
                    case 7:
                        player.MissileActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy_Laser")
        {
            Destroy(this.gameObject);
        }
    }

    public void RushToPlayer()
    {
        float step = _speedToPlayer * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
    }
}
