using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _beamCharge = 100;
    [SerializeField]
    private float _timeWhenFightStart = 5;

    private float _fireRate = 0.20f;
    private float _fireOn = -1f;

    [SerializeField]
    private int _bossHeal = 150;

    [SerializeField]
    private GameObject _bossBeamAttack;
    [SerializeField]
    private GameObject _bossProjectelAttack;

    private UiManager _UiManager;
    private SpawnManager _SpawnManager;
    private Player _player;
    private Animator _animation;
    private AudioSource _audioSource;
    public AudioClip _projectileAudioSource;

    private bool _IsBossAlive = true;
    private bool _BossAttackActive = false;
    // Start is called before the first frame update
    void Start()
    {
        _UiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        _SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animation = GetComponent<Animator>();
        NullCheck();
    }
    private void NullCheck()
    {
        if (_UiManager == null)
        {
            Debug.LogError("The UiManager is null");
        }
        if (_SpawnManager == null)
        {
            Debug.LogError("The SpawnManager is null");
        }
        if (_player == null)
        {
            Debug.LogError("The Player is null");
        }
        if (_animation == null)
        {
            Debug.LogError("The Animator is null");
        }
    }
    void Update()
    {
        WaitTime();
        BossIsDead();
        SecondPhase();
    }
    private void WaitTime()
    {
        if (_IsBossAlive == true)
        {
            if (_timeWhenFightStart >= 5)
            {
                StartCoroutine(TimeCanStart());
            }
            if (_timeWhenFightStart <= 0)
            {
                FirstPhase();
                BossMovement();
                _UiManager.BossFightStart();
            }
        }
    }
    private void BossMovement()
    {
        Vector3 BossMovement = transform.position;
        BossMovement.x = 6f * Mathf.Sin(Time.time * _speed);
        transform.position = BossMovement;
       
    }
    private void FirstPhase()
    {
        if (_bossHeal >= 51)
        {
            if (Time.time > _fireOn && _IsBossAlive == true)
            {
                _speed = 1.2f;
                _fireRate = Random.Range(3f, 7f);
                _fireOn = Time.time + _fireRate;
                Instantiate(_bossProjectelAttack, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_projectileAudioSource);
            }
        }
    }
    private void SecondPhase()
    {
        if(_bossHeal <= 90)
        {
            BossSecondAttack();
        }
    }
    private void BossSecondAttack()
    {
        if (_IsBossAlive == true)
        {
            _speed = 2.5f;
            if (_beamCharge >= 100)
            {
                _BossAttackActive = true;
                _bossBeamAttack.SetActive(true);
                StartCoroutine(BossBeamUsed());
            }
            if (_beamCharge <= 0)
            {
                _BossAttackActive = false;
                _bossBeamAttack.SetActive(false);
                StartCoroutine(BossBeamRestoration());
            }


        }
    }
    IEnumerator TimeCanStart()
    {
        while(_timeWhenFightStart >= 0)
        {
            _timeWhenFightStart -= Time.deltaTime * 10;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator BossBeamUsed()
    {
        while (_BossAttackActive == true && _beamCharge >= 0)
        {
            _beamCharge -= Time.deltaTime * 80;
            yield return new WaitForSeconds(0.1f);
        }
       

    }
    IEnumerator BossBeamRestoration()
    {
        while (_BossAttackActive == false && _beamCharge <= 100)
        {
            _beamCharge += Time.deltaTime * 80;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void BossIsDead()
    {
        if (_bossHeal <= 0)
        {
            Destroy(this.gameObject);
            _player.PlusScore(100);
            _audioSource.Play();
            _IsBossAlive = false;
        }
        if(_IsBossAlive == false)
        {
            _animation.SetTrigger("WhenEnemyDies");
            _SpawnManager.StartOfNormalWave();
            _player.BossReward();
            _UiManager.BossFightOver();
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "laser" && _timeWhenFightStart <= 0)
        {
            _bossHeal -= 5;
            _UiManager.BossHealGoingDown();
            _audioSource.Play();
        }

    }
}

