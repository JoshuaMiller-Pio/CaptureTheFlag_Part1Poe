using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFlash : MonoBehaviour
{
    // Start is called before the first frame update
    private Light _light;
    private bool canshoot = true;
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") >0 && canshoot && !GameManager.Instance.Paused)
        {
            StartCoroutine(flash());
            StartCoroutine(timer());
            canshoot = false;
        }
        
    }

    IEnumerator flash()
    {
        _light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _light.enabled = false;
        yield return null;
    }
    IEnumerator timer()
    {
       
        yield return new WaitForSeconds(1);
        canshoot = true;
        yield return null;
    }
}
