using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject[] PowerUps;
    [SerializeField]
    private GameObject[] Enemy;
    [SerializeField]
    private GameObject _bossPrefab;
    [SerializeField]
    private int _waves = 1;
    [SerializeField]
    private int _numberOfEnemy;
    [SerializeField]
    private int _digit;

    private float _enemySpawnRate = 1f;

    private bool _gameOver = false;
    private bool _isWaveOn = true;
    private bool _stopSpawning = false;
    private bool _startSpwaningEnemy = false;
    private bool _isBossalive = true;
    private bool _isBossFightOn = false;

    private UiManager _uiManager;


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        NullCheck();
        _uiManager.WaveCountText(_waves);


    }
    private void NullCheck()
    {
        if (_uiManager == null)
        {
            Debug.LogError("The UiManager is null");
        }
    }

    void Update()
    {
        if (_startSpwaningEnemy == true)
        {
            StartSpawningEnemy();
        }
    }
    public void StartSpawningEnemy()
    {

        StartCoroutine(ReSpawn());
        
       
    }
    public void StartSpawningPowerUps()
    {
        StartCoroutine(Powerup());
    }


    IEnumerator ReSpawn()
    {
        while (_gameOver == false && _isWaveOn == true)
        {

            _isWaveOn = false;
            if (_waves % 5 == 0 && _isBossalive == true)
            {
                _isBossFightOn = true;
                EndOfNormalWave();
                _stopSpawning = true;
                Vector3 BossSpawn = new Vector3(0, 4, 0);
                GameObject newboss = Instantiate(_bossPrefab, BossSpawn, Quaternion.identity);
            }
           
            else if (_stopSpawning == false)
            {
                Vector3 spawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                for (int i = 0; i < _numberOfEnemy; i++)
                {
                    yield return new WaitForSeconds(1);
                    int RandomEnemy = Random.Range(0, 4);
                    GameObject newenemy = Instantiate(Enemy[RandomEnemy], spawn, Quaternion.identity);
                    newenemy.transform.parent = _container.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    _enemySpawnRate -= 1f;
                }
            }
            if (_isBossFightOn == false)
            {
                _numberOfEnemy += 1;
                yield return new WaitForSeconds(5);
                _waves += 1;
                _uiManager.WaveCountText(_waves);
                _isWaveOn = true;
            }
        }

    }

    IEnumerator Powerup()
    {
        while (_gameOver == false)
        {
            _digit = (Random.Range(0, 101));
            Vector3 spawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int RandomPowerup = Random.Range(0, 5);
            if (_digit <= 10)
            {
                GameObject newpowerups = Instantiate(PowerUps[7], spawn, Quaternion.identity);
                newpowerups.transform.parent = _container.transform;
            }
            else if(_digit <= 21)
            {
                GameObject newpowerups = Instantiate(PowerUps[5], spawn, Quaternion.identity);
                newpowerups.transform.parent = _container.transform;
            }
            else if (_digit <= 61)
            {
                GameObject newpowerups = Instantiate(PowerUps[6], spawn, Quaternion.identity);
                newpowerups.transform.parent = _container.transform;
                
            }
            else
            {
                GameObject newpowerups = Instantiate(PowerUps[RandomPowerup], spawn, Quaternion.identity);
                newpowerups.transform.parent = _container.transform;
            }
            yield return new WaitForSeconds(Random.Range(5, 11));
        }
    }
    public void StartEnemySpawning()
    {
        _startSpwaningEnemy = true;
    }

    private void EndOfNormalWave()
    {

        if (_numberOfEnemy <= 1)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                GameObject.Destroy(enemy);
            }
        }

    }
    public void StartOfNormalWave()
    {
        _uiManager.WaveCountText(_waves);
        _numberOfEnemy += 1;
        _stopSpawning = false;
        _isBossalive = false;
        _isBossFightOn = false;
        _isWaveOn = true;
    }
    
 
    public void Whenpalyerdies()
    { 
        _gameOver = true;
        StopAllCoroutines();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }

    }
}


