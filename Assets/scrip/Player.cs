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

    private float _fireRate = 0.20f;
    private float _fireOn = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UiManager _uiManager;
    private Enemy _enemy;


    private bool _isTripleShotActive = false;
    private bool _isSpeedBootsActive = false;
    private bool _isShieldActive = false;
    private bool _isTheBlastActive = false;
    

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
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        transform.position = new Vector3(0, -2, 0);

    }
    private void NullCheck()
    {
        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UiManager is null");
        }
    }


        void Update()
    {
        movement();
        bounder();

    }

    void movement()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalinput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalinput * _speed * Time.deltaTime);


    }
    void bounder()
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


        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireOn)
            Shot_input();
    }

    private void Shot_input()

    {
        _fireOn = Time.time + _fireRate;
        
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        }

        if (_isTheBlastActive == true)
        {
            Instantiate(_theBlastPrefab, transform.position, Quaternion.identity);
            _isTheBlastActive = false;
            _laserAudioSource.Play();
            return;
        }

        {
            _laserAudioSource.Play();
        }
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        _uiManager.LivesPlayerHas(_lives);

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
            _spawnManager.Whenpalyerdies();
            Destroy(this.gameObject);
          
        }

    }
    public void TripleShotOn()
    {
        _isTripleShotActive = true;
        _powerUpCollect.Play();
        StartCoroutine(TripleShotOff());
    }
    IEnumerator TripleShotOff()
    {
        yield return new WaitForSeconds(6.0f);
        _isTripleShotActive = false;
    }
    public void SpeedUpOn()
    {
        _isSpeedBootsActive = true;
        _powerUpCollect.Play();
        _speed = 5.5f;
        StartCoroutine(SpeedUpoff());

    }
    IEnumerator SpeedUpoff()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedBootsActive = false;
        _speed = 3.5f;
    }
    public void ShieldOn()
    {
        _isShieldActive = true;
        _powerUpCollect.Play();
        _shieldVisualizer.SetActive(true);
    }
    public void TheBlastOn()
    {
        _isTheBlastActive = true;
       
        _powerUpCollect.Play();
    }
   
    public void PlusScore(int points)
    {

        _score += points;
        _uiManager.Score(_score);
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            _explosionAudioSource.Play();
        }
    }
}