using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private int _speed = 3;
    [SerializeField]
    private int _powerupID;

  
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            switch (_powerupID)
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

            }
            Destroy(this.gameObject);

        }
    }
}
