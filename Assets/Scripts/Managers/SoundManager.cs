using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip gun, pickup, start,heal;

    public AudioSource sfx;
    

   

    public void playShot()
    {
        sfx.clip = gun;
        sfx.Play();
    }

    public void playPickup()
    {
        sfx.clip = pickup;
        sfx.Play();
    }
    public void playHeal()
    {
        sfx.clip = heal;
        sfx.Play();
    }

    public void playStart()
    {
        sfx.clip = start;
        sfx.Play();
    }
}
