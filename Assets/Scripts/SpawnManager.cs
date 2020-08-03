﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private int _multiShotToLaunchCount = 2;
    [SerializeField] 
    private int _numberOfWaves = 10;
    [SerializeField]
    private int _waveTimer = 60;
    [SerializeField] 
    private float _timerBetweenWaves;

    private bool _stopSpawning = false;
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        for (int i = 0; i < _numberOfWaves; i++)
        {
            float startTime = Time.time;

            while (((Time.time - startTime) <= _waveTimer) && _stopSpawning == false)
            {
                Debug.Log("Wave: " + (i + 1));
                Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(_numberOfWaves - i);
            }
            yield return new WaitForSeconds(_timerBetweenWaves);
        }
        
        
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            Vector3 posToSpawnPowerup = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);
            int randomPowerup = Random.Range(0, 7);
            if (randomPowerup >= 0 && randomPowerup <= 7)
            {
                GameObject newPowerup = Instantiate(_powerups[randomPowerup], posToSpawnPowerup, Quaternion.identity);
            }
            else if (randomPowerup == 5 && _multiShotToLaunchCount > 0)
            {
                _multiShotToLaunchCount--;
            }
            else if (randomPowerup == 5 && _multiShotToLaunchCount == 0)
            {
                GameObject newPowerup = Instantiate(_powerups[5], posToSpawnPowerup, Quaternion.identity);
                _multiShotToLaunchCount = 2;
            }
            
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
