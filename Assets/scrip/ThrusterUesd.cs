using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ThrusterUesd : MonoBehaviour
{
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrustersBoots();
    }
    void ThrustersBoots()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _image.fillAmount -=  Time.deltaTime * 0.50f;
        }
        else
        {
            _image.fillAmount += Time.deltaTime * 0.20f;
        }
        
    }
}
