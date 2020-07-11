﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;

    private void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); //Current Game scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
}
