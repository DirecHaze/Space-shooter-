using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private int _rotationSpeed = 20;

    [SerializeField]
    private GameObject _explosionPrefab;

    private Player _player;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _explosionAudioSource;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        NullCheck();
    }
    private void NullCheck()
    {
        if (_player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is null");
        }
    }
    private void Update()
    {
        roation();
    }
    void roation()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            if (_player != null)
            {
                _player.PlusScore(5);
            }
            _explosionAudioSource.Play();
            Destroy(other.gameObject);
            _spawnManager.StartEnemySpawning();
            _spawnManager.StartSpawningPowerUps();
            Destroy(this.gameObject, 0.20f);
        }
    }
}