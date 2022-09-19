using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private int _speed = 3;
    [SerializeField]
    private int _powerUpID;

    private float _qickmove = 5.5f;

    private Player _player;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null)
        {
            Debug.LogError("The Player is null");
        }
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
        test();
    }
    public void MoveToPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, _player.transform.position, _qickmove * Time.deltaTime);
    }
    public void test()
    {
        if (Input.GetKey(KeyCode.C))
        {
            MoveToPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            switch (_powerUpID)
            {
                case 0:
                    player.TripleShotOn();
                    break;
                case 1:
                    player.SpeedUpOn();
                    break;
                case 2:
                    player.ShieldOn();
                    break;
                case 3:
                    player.TheBlastOn();
                    break;
                case 4:
                    player.AmmoPickUp();
                    break;
                case 5:
                    player.LifeUP();
                    break;
                case 6:
                    player.MissileOn();
                    break;
                case 7:
                    player.SpeedDownOn();
                    break;

            }
            Destroy(this.gameObject);

        }
    }
}
