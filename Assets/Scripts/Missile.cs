using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Laser
{
    protected override void Update()
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

        FindClosestEnemy();
    }

    private void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }

            //Debug.DrawLine(this.transform.position, closestEnemy.transform.position);

            //Rotate to target
            float x = closestEnemy.transform.position.x - transform.position.x;
            float y = closestEnemy.transform.position.y - transform.position.y;
            float zRotation = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, zRotation - 90f);

        }
    }
}
