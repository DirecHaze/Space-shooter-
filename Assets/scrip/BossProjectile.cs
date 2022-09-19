using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{

    private float _speed = 2;

    private Player _player;

    private GameObject _gameObjectPlayer;


    private AudioSource _AudioSource;

    

   
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _AudioSource = GetComponent<AudioSource>();
        NullCheck();
    }
    void NullCheck()
    {
        if (_player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_AudioSource == null)
        {
            Debug.LogError("The AudioSource is null");
        }
    }
        // Update is called once per frame
    void Update()
    {
        if (_gameObjectPlayer == null)
        {
            _gameObjectPlayer = TrackingPlayer();
        }
        if (_gameObjectPlayer != null)
        {
            LockOnTarget();
        }
        ProjectileMovent();
        Destroy();
    }
    private void ProjectileMovent()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
    private void Destroy()
    {
        Destroy(this.gameObject, 2);
    }
    private GameObject TrackingPlayer()
    {

        try
        {
            GameObject[] players;
            players = GameObject.FindGameObjectsWithTag("Player");

            GameObject Traget = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject Player in players)
            {
                Vector3 diff = Player.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    Traget = Player;
                    distance = curDistance;
                }

            }
            return Traget;
        }
        catch
        {
            return null;
        }

    }
    private void LockOnTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _gameObjectPlayer.transform.position, _speed * Time.deltaTime);
        Vector3 offset = transform.position - _gameObjectPlayer.transform.position;

        transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), offset);
        if (Vector3.Distance(transform.position, _gameObjectPlayer.transform.position) < 0.001f)
        {
            _gameObjectPlayer.transform.position *= -1.0f;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.DamageForBoss();

            }
            _AudioSource.Play();
            Destroy(this.gameObject);

        }
        if (other.tag == "PowerUps")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
