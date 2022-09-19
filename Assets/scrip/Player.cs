using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoots = 1;
    [SerializeField]
    private float _thrusterBoots = 1;
    [SerializeField]
    private float _thrusterCharge = 100;

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
    private GameObject _ammoPrefab;
    [SerializeField]
    private GameObject _1UpPrefab;
    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private GameObject _slowSpeedPrefab;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;
    [SerializeField]
    private GameObject _rightThruster;
    [SerializeField]
    private GameObject _leftThruster;

    [SerializeField]
    private int _score;

    private float _fireRate = 0.20f;
    private float _fireOn = -1f;
    [SerializeField]
    private int _numberOfLaserShots = 15;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shieldStr;

    [SerializeField]
    private int _lifeForBossFight = 200;
    [SerializeField]
    private int _bossShieldStr;

    private SpawnManager _spawnManager;
    private UiManager _uiManager;
    private CamShake _cam;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBootsActive = false;
    private bool _isShieldActive = false;
    private bool _isTheBlastActive = false;
    private bool _isAmmoPickUpActive = false;
    private bool _isMissileActive = false;
    private bool _playerIsDead = false;
    private bool _thusterActive = false;
    private bool _isSlowSpeedActive = false;
   

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
        _cam = GameObject.Find("MainCamera").GetComponent<CamShake>();
        transform.position = new Vector3(0, -2, 0);
        NullCheck();
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
        if (_shieldRenderer == null)
        {
            Debug.LogError("Shield NOt Working My G");
        } 
        if (_cam == null)
        {
            Debug.LogError("Cam not working my G");
        }
    }

    void Update()
    {
        Movement();
        Boundaries();
        PlayerIsDead();
        ThrustersBoots();
        ThrustersUsed();
        ThrustersRestoration();
    }

    void Movement()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(horizontalinput, verticalinput, 0);
        
        transform.Translate(Direction * _speed * _speedBoots * _thrusterBoots * Time.deltaTime);

    }
    void ThrustersBoots()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterCharge > 0)
        {
            _thrusterBoots = 2;
            _thusterActive = true;
        }
        else
        {
            _thrusterBoots = 1;
            _thusterActive = false;
        }
        if(_thrusterCharge < 5)
        {
            _thusterActive = false;
            _thrusterBoots = 1;
        }
    }
    private void ThrustersUsed()
    {
        if (_thusterActive == true && _thrusterCharge >= 0)
        {
            _thrusterCharge -= Time.deltaTime * 50;
        }
    }
    private void ThrustersRestoration()
    {
        if (_thusterActive == false && _thrusterCharge <= 100)
        {
            _thrusterCharge += Time.deltaTime * 20;
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireOn)
        {
            if (_numberOfLaserShots == 0)
            {
                return;
            }
            Shot_input();
        }
        if (Input.GetKeyDown(KeyCode.X) && Time.time > _fireOn)
        {
            if (_numberOfLaserShots == 0)
            {
                return;
            }
            MissileShotInput();
        }
    }

    private void Shot_input()

    {
      
        _fireOn = Time.time + _fireRate;
        LaserCount(-1);
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

    private void MissileShotInput()
    {
        _fireOn = Time.time + _fireRate;
        if (_isMissileActive == true)
        {
            Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            _isMissileActive = false;
            _laserAudioSource.Play();
            _uiManager.WhenXIsPress();
            return;
        }
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            if (_shieldStr == 3)
            {
                _shieldStr--;
                _shieldRenderer.color = Color.yellow;
                return;

            }
            if (_shieldStr == 2)
            {
                _shieldStr--;
                _shieldRenderer.color = Color.red;
                return;

            }
            if (_shieldStr == 1)
            {
                _shieldStr--;
                _bossShieldStr = 0;
                _shieldVisualizer.SetActive(false);
                return;

            }

        }
    

        _lives--;
        _uiManager.LivesPlayerHas(_lives);
        _cam.CamShakeOn();

        if (_lives < 3)
        {
            _rightThruster.SetActive(true);
            _lifeForBossFight = 100;
        }
        if (_lives < 2)
        {
            _leftThruster.SetActive(true);
            _lifeForBossFight = 50;
        }
        if (_lives < 1)
        {
            
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _explosionAudioSource.Play();
            _spawnManager.Whenpalyerdies();
            _playerIsDead = true;

        }

    }
    public void DamageForBoss()
    {
        if (_isShieldActive == true)
        {
            if (_bossShieldStr == 51)
            {
                _bossShieldStr -= 17;
                _shieldRenderer.color = Color.yellow;
                return;

            }
            if (_bossShieldStr == 34)
            {
                _bossShieldStr -= 17;
                _shieldRenderer.color = Color.red;
                return;

            }
            if (_bossShieldStr == 17)
            {
                _bossShieldStr -= 17;
                _shieldStr = 0;
                _shieldVisualizer.SetActive(false);
                return;

            }

        }
        _cam.CamShakeOn();
        PlayerHealthForBoss(-10);
        _uiManager.LivesPlayerHas(_lives);
        if (_lifeForBossFight == 100)
        {
            _rightThruster.SetActive(true);
            _lives = 2;
        }
        if (_lifeForBossFight == 50)
        {
            _leftThruster.SetActive(true);
            _lives = 1;
        }
        if (_lifeForBossFight <= 0)
        {

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _lives = 0;
            _uiManager.LivesPlayerHas(_lives);
            _explosionAudioSource.Play();
            _spawnManager.Whenpalyerdies();
            _playerIsDead = true;

        }


    }
    private void PlayerIsDead()
    {
        if (_playerIsDead == true)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            _explosionAudioSource.Play();
        }
        if (other.tag == "BossBeam")
        {
            DamageForBoss();
        }
        if (other.tag == "BossProjectile")
        {
            Destroy(other.gameObject);
        }
    }
    public void TripleShotOn()
    {
        _isTripleShotActive = true;
        _numberOfLaserShots = 99;
        _uiManager.UpdateAmmoCount(_numberOfLaserShots);
        _powerUpCollect.Play();
        StartCoroutine(TripleShotOff());
    }
    IEnumerator TripleShotOff()
    {
        yield return new WaitForSeconds(6.0f);
        _isTripleShotActive = false;
        _numberOfLaserShots = 15;
        _uiManager.UpdateAmmoCount(_numberOfLaserShots);
    }
    public void SpeedUpOn()
    {
        _isSpeedBootsActive = true;
        _powerUpCollect.Play();
        _speedBoots = 1.5f;
        StartCoroutine(SpeedUpoff());

    }
    IEnumerator SpeedUpoff()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedBootsActive = false;
        _speedBoots = 1;
    }
    public void ShieldOn()
    {
        _shieldStr = 3;
        _bossShieldStr = 51;
        _powerUpCollect.Play();
        _shieldVisualizer.SetActive(true);
        _shieldRenderer.color = Color.cyan;
        _isShieldActive = true; 

     
    }
    public void TheBlastOn()
    {
        _isTheBlastActive = true;

        _powerUpCollect.Play();
    }
    public void MissileOn()
    {
        _isMissileActive = true;
        if (_isMissileActive == true)
        {
            _powerUpCollect.Play();
            _uiManager.PressXCalled();
        }
    }
    public void SpeedDownOn()
    {
        _isSlowSpeedActive = true;
        _powerUpCollect.Play();
        _speed = 1.5f;
        StartCoroutine(SpeedDownoff());

    }
    IEnumerator SpeedDownoff()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedBootsActive = false;
        _speedBoots = 1;
        _speed = 3.5f;
    }
    public void AmmoPickUp()
    {
        _isAmmoPickUpActive = true;
        _numberOfLaserShots = 15;
        _uiManager.UpdateAmmoCount(_numberOfLaserShots);
    }
    public void LifeUP()
    {
       
        if (_lives == 1)
        {
            _lives++;
            _uiManager.LivesPlayerHas(_lives);
            _leftThruster.SetActive(false);
            return;
        }
        if (_lives == 2)
        {
            _lives++;
            _uiManager.LivesPlayerHas(_lives);
            _rightThruster.SetActive(false);
            return;
        }
        if(_lifeForBossFight <= 100)
        {
            _lifeForBossFight += 25;
            _uiManager.PlayerHealth(_lifeForBossFight);
        }
        if(_lifeForBossFight >= 100)
        {
            _lifeForBossFight = 100;
        }
       
    }
    public void PlusScore(int points)
    {

        _score += points;
        _uiManager.Score(_score);

    }
    public void LaserCount(int Ammo)
    {
        _numberOfLaserShots += Ammo;
        _uiManager.UpdateAmmoCount(_numberOfLaserShots);
    }
    public void PlayerHealthForBoss(int Health)
    {
        _lifeForBossFight += Health;
        _uiManager.PlayerHealth(_lifeForBossFight);
    }
    public void BossReward()
    {
        _lifeForBossFight = 200;
        _uiManager.PlayerHealth(_lifeForBossFight);
        _lives = 3;
        _uiManager.LivesPlayerHas(_lives);
        _leftThruster.SetActive(false);
        _rightThruster.SetActive(false);
    }
    public void ShelidDamageForEnemy()
    {

        if (_shieldStr == 0)
        {
            _shieldVisualizer.SetActive(false);
        }
    }
}