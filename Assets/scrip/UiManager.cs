using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesLeftImg;
    [SerializeField]
    private Image _bossHealImage;

    [SerializeField]
    private Sprite[] _livesDisplay;

    [SerializeField]
    private Text _numberOfAmmo;
    [SerializeField]
    private Text _pressX;
    [SerializeField]
    private Text _playerHealthForBoss;
    [SerializeField]
    private TextMeshProUGUI _waveTexts;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;


    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _scoreText.text = "Score: " + 0;
        _numberOfAmmo.text = "15 / " + 15.ToString();
        NullCheck();
        transform.GetChild(8).gameObject.SetActive(false);
    }
    private void NullCheck()
    {
        if (_gameManager == null)
        {
            Debug.LogError("The GameManager is null");
        }

    }
    public void Score(int Points)
    {
        _scoreText.text = "Score: " + Points;
    }
    public void UpdateAmmoCount(int Ammo)
    {
        _numberOfAmmo.text = "15 / " +  Ammo.ToString();
    }
    public void PressXCalled()
    {
        _pressX.text = "Press X: For Missile";
    }
    public void WhenXIsPress()
    {
        _pressX.text = "     ";
    }
    public void BossFightStart()
    {
        transform.GetChild(8).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void BossFightOver()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(8).gameObject.SetActive(false);
    }
    public void WaveCountText(int Wave)
    {
        _waveTexts.text = "Wave: " + Wave.ToString();
        
    }
   

    
    public void PlayerHealth(int Health)
    {
        _playerHealthForBoss.text = "Health: " + Health.ToString();
    }
    public void BossHealGoingDown()
    {
        _bossHealImage.fillAmount -= 0.025f;
        if(_bossHealImage.fillAmount <= 0.5000005)
        {
            _bossHealImage.color = Color.yellow;
        }
        if(_bossHealImage.fillAmount <= 0.2440004)
        {
            _bossHealImage.color = Color.red;
        }
    }

    public void LivesPlayerHas(int NumbersOfLives)
    {
        _livesLeftImg.sprite = _livesDisplay[NumbersOfLives];

        if (NumbersOfLives == 0)
        {
            StartCoroutine(GameOver());
        }
    }
    IEnumerator GameOver()
    {
        while (true)
        {
            _gameManager.GAMEOver();
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            transform.GetChild(2).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}


