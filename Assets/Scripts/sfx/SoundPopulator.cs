using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPopulator : MonoBehaviour
{
    public AudioClip gun, pickup, start, heal;

    public AudioSource sfx;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.sfx = sfx;
        SoundManager.Instance.gun = gun;
        SoundManager.Instance.pickup = pickup;
        SoundManager.Instance.start = start;
        SoundManager.Instance.heal = heal;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
