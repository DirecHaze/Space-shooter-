using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.5f;

    private GameObject _gameObjectPlayer;

    private Player _player;
    private Animator _animation;
    private AudioSource _audioSource;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _damageShip;

    private bool _IsenemyAlive = true;
    private bool _SelfDestroy = true;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animation = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        NullCheck();
    }
    private void NullCheck()
    {
        if (_player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_animation == null)
        {
            Debug.LogError("The Animator is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is null");
        }
        if (_gameManager == null)
        {
            Debug.LogError("The GameManager is null");
        }

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
            return Traget ;
        }
        catch
        {
            return null;
        }

    }

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
        Move();
        SelfDestroy();
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
    private void Move()
    {
 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
    private void SelfDestroy()
    {
        if (_SelfDestroy == true)
        {
            Invoke("Anima", 7f);
            Destroy(this.gameObject, 8);

        }
        else if (_SelfDestroy == false)
        {
            return;
        }
    }
    private void Anima()
    {
        if (_SelfDestroy == true)
        {
            _animation.SetTrigger("WhenEnemyDies");
            Destroy(GetComponent<Collider2D>());
            _speed = 0;
            _damageShip.SetActive(false);
            _audioSource.Play();
            AudioSource.Destroy(this.gameObject, 0.8f);
            return;
        }
        else if (_SelfDestroy == false)
        {
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();

            }
            _player.ShelidDamageForEnemy();
            _animation.SetTrigger("WhenEnemyDies");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _IsenemyAlive = false;
            _SelfDestroy = false;
            _damageShip.SetActive(false);
            Destroy(this.gameObject, 2.9f);
        }
        if (other.tag == "laser")
        {

            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.PlusScore(10);
            }
            _animation.SetTrigger("WhenEnemyDies");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _IsenemyAlive = false;
            _SelfDestroy = false;
            _damageShip.SetActive(false);
            Destroy(this.gameObject, 2.9f);
        }
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _animation.SetTrigger("WhenEnemyDies");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _IsenemyAlive = false;
            _SelfDestroy = false;
            _damageShip.SetActive(false);
            Destroy(this.gameObject, 2.9f);
        }

    }
}
