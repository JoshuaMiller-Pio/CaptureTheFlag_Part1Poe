using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(popup());
    }
    IEnumerator popup()
    {
        yield return new WaitForSeconds(1.5f);
        
        gameObject.SetActive(false);
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
