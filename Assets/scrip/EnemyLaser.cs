using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    private int _speed = 8;

    private Player _player;

    private AudioSource _audioSource;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        NullCheck();
    }
    void NullCheck()
    {
        if (_player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is null");
        }
    }
    void Update()
    {
        Velocity();
        Boundaries();
    }

    private void Boundaries()
    {
        if (transform.position.y <= -8)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);

            }
            Destroy(gameObject);
        }
    }
    void Velocity()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();

            }
            _audioSource.Play();
            Destroy(this.gameObject);
            
        }
        if (other.tag == "PowerUps")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }    
    }
} 
