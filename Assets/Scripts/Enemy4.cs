using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy4 : Enemy
{

    protected override void Movement()
    {
        float x = _target.position.x - transform.position.x;
        float y = _target.position.y - transform.position.y;
        float zRotation = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRotation + 90f);

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
            yield return new WaitForSeconds(2.0f);
            if (_hit == false)
                FireLaser();
        }
    }
}
