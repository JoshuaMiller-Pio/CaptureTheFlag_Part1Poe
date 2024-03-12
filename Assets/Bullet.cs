using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(waitToDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        
        yield return null;
    }
}
