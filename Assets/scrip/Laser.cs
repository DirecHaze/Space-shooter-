using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private int _speed = 8;



    // Update is called once per frame
    void Update()
    {
        Velocity();


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
