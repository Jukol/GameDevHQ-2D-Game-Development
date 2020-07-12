using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _outOfAmmo;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    [SerializeField]
    private GameManager _gameManager;
    private Player _player;

 
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _restartText.SetActive(false);
        _outOfAmmo.gameObject.SetActive(false);
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];
    }

    public void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartText.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
    }

    
    public void OutOfAmmoFlicker()
    {
        StartCoroutine(OutOfAmmoFlickerText());
    }
    
    IEnumerator OutOfAmmoFlickerText()
    {
        int i = 3;
        while (i > 0)
        {
            _outOfAmmo.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _outOfAmmo.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            i--;
            
        }
        _player.flickerStarted = false;
    }
}
