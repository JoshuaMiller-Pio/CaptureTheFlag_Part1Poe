using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update

    public void Play()
    {
        GameManager.Instance.Play();
    }

    public void Restart()
    {
        
        GameManager.Instance.Restart();
    }

    public void Quit()
    {
        
        GameManager.Instance.Quit();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}


