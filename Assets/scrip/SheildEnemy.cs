using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;

    [SerializeField]
    private GameObject _enemylaserPrefab;
    [SerializeField]
    private GameObject _enemySheildVisualizer;

    private float _fireRate = 0.20f;
    private float _fireOn = -1f;

    private Player _player;
    private Animator _animation;
    private AudioSource _audioSource;
    private GameManager _gameManager;

    public AudioClip _laserAudioSource;

    private bool _isSheildEnemyAlive = true;
    private bool _selfDestroy = true;

    private int _randomMove;
    [SerializeField]
    private int _sheildEnemyStr = 2;

    void Start()
    {

        StartCoroutine(RandomMove());
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
        _selfDestroy = true;
    }

    void Update()
    {

        Move();
        RandomSpawn();
        SheildEnemyshoot();
        NewMovement();
    }
    private void SheildEnemyshoot()
    {

        if (Time.time > _fireOn && _isSheildEnemyAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _fireOn = Time.time + _fireRate;
            Instantiate(_enemylaserPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserAudioSource);

        }
    }

    private void Move()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

    }
    private void RandomSpawn()
    {
        if (transform.position.y <= -5.6f)

        {
            float RandomSpawn = Random.Range(-8f, 8f);

            transform.position = new Vector3(RandomSpawn, 7, 0);
        }

    }

    private void NewMovement()
    {
        switch (_randomMove)
        {
            case 0:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                break;
            case 2:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                break;
        }
    }

    IEnumerator RandomMove()
    {
        yield return new WaitForSeconds(1);
        while (_isSheildEnemyAlive == true)
        {
            _randomMove = Random.Range(0, 3);
            yield return new WaitForSeconds(4);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_sheildEnemyStr == 2)
        {
            if (other.tag == "laser")
            {
                _sheildEnemyStr--;
                _enemySheildVisualizer.SetActive(false);
                _audioSource.Play();
                Destroy(other.gameObject);
                return;
            }
            if (other.tag == "Player")
            {
                if (_player != null)
                {
                    _player.Damage();
                    
                }
                _player.ShelidDamageForEnemy();
                return;
            }
            if (other.tag == "Missile")
            {
                _audioSource.Play();
                _sheildEnemyStr--;
                _enemySheildVisualizer.SetActive(false);
                Destroy(other.gameObject);
                return;
            }

        }
        if (_sheildEnemyStr == 1)
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
                _isSheildEnemyAlive = false;
                _selfDestroy = false;
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
                _isSheildEnemyAlive = false;
                _selfDestroy = false;
                Destroy(this.gameObject, 2.9f);
            }
            if (other.tag == "Missile")
            {
                Destroy(other.gameObject);
                _animation.SetTrigger("WhenEnemyDies");
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                _isSheildEnemyAlive = false;
                _selfDestroy = false;
                Destroy(this.gameObject, 2.9f);
            }
        }

    }
}
