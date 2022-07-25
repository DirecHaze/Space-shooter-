using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private int _rotationSpeed = 20;

    [SerializeField]
    private GameObject _explosionPrefab;

    private Player _Player;
    private SpawnManager _SpawnManager;

    [SerializeField]
    private AudioSource _explosionAudioSource;
    void Start()
    {
        _Player = GameObject.Find("Player").GetComponent<Player>();
        _SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        NullCheck();
    }
    private void NullCheck()
    {
        if (_Player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_SpawnManager == null)
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
            if (_Player != null)
            {
                _Player.PlusScore(5);
            }
            _explosionAudioSource.Play();
            Destroy(other.gameObject);
            _SpawnManager.StartSpawning();
            Destroy(this.gameObject, 0.20f);
        }
    }
}