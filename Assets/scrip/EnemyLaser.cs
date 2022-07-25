using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    private int _speed = 8;

    private Player _Player;

    private AudioSource _AudioSource;

    void Start()
    {
        _Player = GameObject.Find("Player").GetComponent<Player>();
        _AudioSource = GetComponent<AudioSource>();
        NullCheck();
    }
    void NullCheck()
    {
        if (_Player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_AudioSource == null)
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
            if (_Player != null)
            {
                _Player.Damage();

            }
            _AudioSource.Play();
            Destroy(this.gameObject);

        }
    }
} 
