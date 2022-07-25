using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private int _speed = 8;



    void Update()
    {
        Velocity();
        Boundaries();
    }
    void Boundaries()
    {
        if (transform.position.y >= 8)
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
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
