using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;

    [SerializeField]
    private GameObject _enemylaserPrefab;

    private float _fireRate = 0.20f;
    private float _fireOn = -1f;

    private Player _player;
    private Animator _animation;
    private AudioSource _audioSource;
    private GameManager _gameManager;
    [SerializeField]
    private SpriteRenderer _smartEnemyRenderer;

    public AudioClip _laserAudioSource;

    private bool _IsEnemyAlive = true;
    private bool _SelfDestroy = true;
   
    private bool _IsEnemyBehindThePlayer = false;
    private bool _IsPowerUpsNearTheEnemy = false;
    private bool _IsLaserNear = false;
        
    private int _randomMove;
    private int _digit;

    [SerializeField]
    private float _rayCastRad = 0.9f;

    void Start()
    {

        StartCoroutine(RandomMove());
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animation = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _smartEnemyRenderer = GetComponent<SpriteRenderer>();
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
        RandomSpawn();
        NewMovement();
        BackShotsAttack();
        EnemySeesPlayer();
        EnemyAttackPowerUps();
        EnemySeesLaser();
    }
   
   

    private void Move()
    {
        if (_IsLaserNear == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

        }

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
        while (_IsEnemyAlive == true && _IsLaserNear == false)
        {
            _randomMove = Random.Range(0, 3);
            yield return new WaitForSeconds(4);
        }
    }
    private void EnemySeesPlayer()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayCastRad, Vector2.down, 4f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Player") && Time.time > _fireOn)
            {
                Debug.Log("Got him");
                enemyshoot();

            }

        }
    }
    private void enemyshoot()
    {
       

        if (Time.time > _fireOn && _IsEnemyAlive == true && _IsEnemyBehindThePlayer == false && _IsPowerUpsNearTheEnemy == false)
        {
            _fireRate = Random.Range(1f, 4f);
            _fireOn = Time.time + _fireRate;
            Instantiate(_enemylaserPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserAudioSource);
            _smartEnemyRenderer.flipY = true;
            return;
        }
    }
    private void BackShotsAttack()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayCastRad, Vector2.up, 6.0f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Player") && Time.time > _fireOn)
            {
                Debug.Log("Got him");
                LaserFireBackward();
                _IsEnemyBehindThePlayer = true;
            }
            else
            {
                _IsEnemyBehindThePlayer = false;
            }
        }
    }
    private void LaserFireBackward()
    {
       
        if (Time.time > _fireOn && _IsEnemyAlive == true && _IsEnemyBehindThePlayer == true)
        {
            _fireRate = Random.Range(1f, 3f);
            _fireOn = Time.time + _fireRate;
            Instantiate(_enemylaserPrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180.0f));
            _audioSource.PlayOneShot(_laserAudioSource);
            _smartEnemyRenderer.flipY = false;
            return;
        }
    }
    private void EnemyAttackPowerUps()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayCastRad, Vector2.down, 6.0f, LayerMask.GetMask("PowerUps"));
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("PowerUps") && Time.time > _fireOn)
            {
                Debug.Log("Got him");
                LaserFireAtPowerUps();
                _IsPowerUpsNearTheEnemy = true;
                _IsEnemyBehindThePlayer = false;
            }
            else
            {
                _IsPowerUpsNearTheEnemy = false;
            }
        }
    }
    private void LaserFireAtPowerUps()
    {

        if (Time.time > _fireOn && _IsEnemyAlive == true && _IsEnemyBehindThePlayer == false && _IsPowerUpsNearTheEnemy == true)
        {
            _fireRate = Random.Range(1f, 2f);
            _fireOn = Time.time + _fireRate;
            Instantiate(_enemylaserPrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0));
            _audioSource.PlayOneShot(_laserAudioSource);
            _smartEnemyRenderer.flipY = true;
        }
    }
    private void EnemySeesLaser()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.down, 1.5f, LayerMask.GetMask("Laser"));
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("laser") && Time.time > _fireOn)
            {

                EnemyDodge();
                _IsLaserNear = true;
            }
            else
            {
                _IsLaserNear = false;
            }

        }
    }
    private void EnemyDodge()
    {

        if (_IsLaserNear == true)
        {
            _digit = (Random.Range(1, 3));
            if (_digit == 1)
            {
                transform.position = new Vector3(transform.position.x + 1.2f, transform.position.y, 0);
                return;
            }
            if (_digit == 2)
            {
                transform.position = new Vector3(transform.position.x - 1.2f, transform.position.y, 0);
                return;
            }
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
            _IsEnemyAlive = false;
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
            _IsEnemyAlive = false;
            _SelfDestroy = false;
            Destroy(this.gameObject, 2.9f);
        }
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _animation.SetTrigger("WhenEnemyDies");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _IsEnemyAlive = false;
            _SelfDestroy = false;
            Destroy(this.gameObject, 2.9f);
        }

    }
}
