using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlag : Flags
{
    public  GameObject Restriction;
    public GameObject Spawn;
    public event EventHandler OnBlueFlagPickup;
    
    void Awake()
    {

        Restriction = GameObject.FindGameObjectWithTag("AI");
        Restricted = Restriction;
        IsAtBase = true;
        Spawnlocation = Spawn.transform.position;
        Debug.Log(Spawn);

    }

    private void Start()
    {
        OnflagPickedUp += BluePickup;

    }

 

   
    private void BluePickup(object sender, EventArgs e)
    {
        OnBlueFlagPickup?.Invoke(this, EventArgs.Empty);
    }
    override 
        public void Respawn()
    {
        gameObject.transform.position = Spawn.transform.position;
        gameObject.transform.rotation = Spawn.transform.rotation;
    }
  
}
