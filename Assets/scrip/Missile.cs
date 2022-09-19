using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 170f;
  
    private GameObject _enemy;

    private GameObject NearestEnemy()
    {

        try
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("enemy");

            GameObject closest = null;
            float distance = Mathf.Infinity;

            Vector3 position = transform.position;
            foreach (GameObject enemy in enemies)
            {
                Vector3 diff = enemy.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = enemy;
                    distance = curDistance;
                }
            }
            return closest;
        }
        catch
        {
            return null;
        }
    }

    void Update()
    {
        if (_enemy == null)
        {
            _enemy = NearestEnemy();
        }
        if (_enemy != null)
        {
            LockOnTarget();
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (transform.position.y > 15)
        {
            Destroy(gameObject);
        }

    }

    public void LockOnTarget()
    {

        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, _speed * Time.deltaTime);
        Vector3 offset = transform.position - _enemy.transform.position;

        transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), offset);
        if (Vector3.Distance(transform.position, _enemy.transform.position) < 0.001f)
        {
            _enemy.transform.position *= -1.0f;
        }

    }
}

