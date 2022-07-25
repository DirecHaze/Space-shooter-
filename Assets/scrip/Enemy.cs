using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _speed = 3;

    [SerializeField]
    private GameObject _EnemylaserPrefab;

    private float _fireRate = 0.20f;
    private float _fireOn = -1f;

    private Player _player;
    private Animator _animation;
    private AudioSource _audioSource;
    private GameManager _gameManager;

    public AudioClip _laserAudioSource;

    private bool _isenemyAlive = true;
    private bool _SelfDestroy = true;
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        _animation = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        NullCheck();
        SelfDestroy();
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
    private void SelfDestroy()
    {
        Destroy(this.gameObject, 37.4f);
        _SelfDestroy = true;
    }

    void Update()
    {

        Move();
        randomSpawn();
        enemyshoot();
    }
    void enemyshoot()
    {

        if (Time.time > _fireOn && _isenemyAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _fireOn = Time.time + _fireRate;
            Instantiate(_EnemylaserPrefab, transform.position, Quaternion.identity);
            if (_isenemyAlive == true)
            {
                _audioSource.PlayOneShot(_laserAudioSource);
            }
        }
    }

    void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

    }
    void randomSpawn()
    {
        if (transform.position.y <= -5.6f)

        {
            float RandomSpawn = Random.Range(-8f, 8f);

            transform.position = new Vector3(RandomSpawn, 7, 0);
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
            _animation.SetTrigger("WhenEnemyDies");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _isenemyAlive = false;
            _SelfDestroy = false;
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
            _isenemyAlive = false;
            _SelfDestroy = false;
            Destroy(this.gameObject, 2.9f);
        }
    }
    public void EndOfEnemy()
    {
        Destroy(this.gameObject);
    }
}






