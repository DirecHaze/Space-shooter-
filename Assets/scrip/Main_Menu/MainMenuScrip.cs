using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScrip : MonoBehaviour
{
    [SerializeField]
    private GameObject _Buttion;
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}