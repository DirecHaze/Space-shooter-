using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScrip : MonoBehaviour
{
    [SerializeField]
    private GameObject _Buttion;
    // Start is called before the first frame update
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}