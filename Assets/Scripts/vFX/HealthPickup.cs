using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private Light _light;

    private MeshRenderer _meshRenderer;

    private SphereCollider _sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _sphereCollider = GetComponent<SphereCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(rotate());
    }

    IEnumerator rotate()
    {
        transform.Rotate(0,0,50*Time.deltaTime);
        yield return null;
    }
    IEnumerator coolDown()
    {
        yield return new WaitForSeconds(5);
        
        _light.enabled =true;
        _meshRenderer.enabled = true;
        _sphereCollider.enabled = true;
        yield return null;
    }
    
    //rotatate
    //disable
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "AI")
        {
            _light.enabled =false;
            _meshRenderer.enabled = false;
            _sphereCollider.enabled = false;
            StartCoroutine(coolDown());

        }
    }
}
