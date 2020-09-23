using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleColliderHolder : MonoBehaviour
{
    private Enemy5 _parentEnemy;

    private void Start()
    {
       _parentEnemy = transform.parent.gameObject.GetComponent<Enemy5>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            float laserX = other.transform.position.x;
            _parentEnemy.DetectPlayersLaser(laserX);
        }
    }
}
