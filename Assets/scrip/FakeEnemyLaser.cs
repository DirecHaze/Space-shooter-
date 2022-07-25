using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemyLaser : MonoBehaviour
{
    private int _speed = 8;

    private Player _Player;



    void Start()
    {
        _Player = GameObject.Find("Player").GetComponent<Player>();
        NullCheck();
    }

    private void NullCheck()
    {
        if (_Player == null)
        {
            Debug.LogError("The Player is null");
        }
    }
    void Update()
    {
        Velocity();
        Boundaries();
    }
    void Boundaries()
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
                Destroy(this.gameObject);


            }


        }
    }
} 