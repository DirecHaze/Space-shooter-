using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject[] powerUps;
    private bool _GameOver = false;
    // Start is called before the first frame update


    public void StartSpawning()
    {

        StartCoroutine(ReSpawn());
        StartCoroutine(Powerup());
    }

    // Update is called once per frame
    void Update()
    {


    }


    IEnumerator ReSpawn()
    {
        while (_GameOver == false)
        {

            Vector3 spawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newenemy = Instantiate(_enemyPrefab, spawn, Quaternion.identity);
            newenemy.transform.parent = _container.transform;
            yield return new WaitForSeconds(4.0f);
        }
    }

    IEnumerator Powerup()
    {
        while (_GameOver == false)
        {
            yield return new WaitForSeconds(2.0f);

            Vector3 spawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int RandomPowerup = Random.Range(0, 4);
            GameObject newpowerups = Instantiate(powerUps[RandomPowerup], spawn, Quaternion.identity);
            newpowerups.transform.parent = _container.transform;
            yield return new WaitForSeconds(Random.Range(6, 11));
        }
    }
    public void Whenpalyerdies()
    {
        _GameOver = true;
        StopAllCoroutines();
    }
}


