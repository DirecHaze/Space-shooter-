using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _speedBoostPrefab;
    [SerializeField]
    private GameObject _shieldUpPrefab;
    [SerializeField]
    private GameObject _theBlastPrefab;


    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightThruster;
    [SerializeField]
    private GameObject _leftThruster;

    [SerializeField]
    private int _score;

    private float _FireRate = 0.20f;
    private float _FireOn = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _SpawnManager;
    private UiManager _UiManager;

    private bool _IsTripleShotActive = false;
    private bool _IsSpeedBootsActive = false;
    private bool _IsShieldActive = false;
    private bool _IsTheBlastActive = false;
    private bool _PlayerIsDead = false;



    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioSource _laserAudioSource;
    [SerializeField]
    private AudioSource _explosionAudioSource;
    [SerializeField]
    private AudioSource _powerUpCollect;

    void Start()
    {

        _SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _UiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        transform.position = new Vector3(0, -2, 0);
        NullCheck();
    }
    private void NullCheck()
    {
        if (_SpawnManager == null)
        {
            Debug.LogError("The SpawnManager is null");
        }

        if (_UiManager == null)
        {
            Debug.LogError("The UiManager is null");
        }
    }

    void Update()
    {
        Movement();
        Boundaries();
        PlayerIsDead();
    }

    void Movement()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalinput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalinput * _speed * Time.deltaTime);

    }
    void Boundaries()
    {
        if (transform.position.y >= 0)

        transform.position = new Vector3(transform.position.x, 0, 0);
    
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        if (transform.position.x >= 16.5f)
        {
            transform.position = new Vector3(-15.5f, transform.position.y, 0);
        }
        else if (transform.position.x <= -16.5f)
        {
            transform.position = new Vector3(16.5f, transform.position.y, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _FireOn)
                Shot_input();
    }

    private void Shot_input()

    {
        _FireOn = Time.time + _FireRate;

        if (_IsTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        }

        if (_IsTheBlastActive == true)
        {
            Instantiate(_theBlastPrefab, transform.position, Quaternion.identity);
            _IsTheBlastActive = false;
            _laserAudioSource.Play();
            return;
        }

        {
            _laserAudioSource.Play();
        }
    }
    public void Damage()
    {
        if (_IsShieldActive == true)
        {
            _IsShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        _UiManager.LivesPlayerHas(_lives);

        if (_lives < 3)
        {
            _rightThruster.SetActive(true);
        }
        if (_lives < 2)
        {
            _leftThruster.SetActive(true);
        }
        if (_lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _explosionAudioSource.Play();
            _SpawnManager.Whenpalyerdies();
            _PlayerIsDead = true;

        }

    }
    private void PlayerIsDead()
    {
        if (_PlayerIsDead == true)
        {
            Destroy(this.gameObject);
        }
    }
    public void TripleShotOn()
    {
        _IsTripleShotActive = true;
        _powerUpCollect.Play();
        StartCoroutine(TripleShotOff());
    }
    IEnumerator TripleShotOff()
    {
        yield return new WaitForSeconds(6.0f);
        _IsTripleShotActive = false;
    }
    public void SpeedUpOn()
    {
        _IsSpeedBootsActive = true;
        _powerUpCollect.Play();
        _speed = 5.5f;
        StartCoroutine(SpeedUpoff());

    }
    IEnumerator SpeedUpoff()
    {
        yield return new WaitForSeconds(7.0f);
        _IsSpeedBootsActive = false;
        _speed = 3.5f;
    }
    public void ShieldOn()
    {
        _IsShieldActive = true;
        _powerUpCollect.Play();
        _shieldVisualizer.SetActive(true);
    }
    public void TheBlastOn()
    {
        _IsTheBlastActive = true;

        _powerUpCollect.Play();
    }

    public void PlusScore(int points)
    {

        _score += points;
        _UiManager.Score(_score);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            _explosionAudioSource.Play();
        }
    }
}