using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesLeftImg;

    [SerializeField]
    private Sprite[] _livesDisplay;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _scoreText.text = "Score: " + 0;
        NullCheck();
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


