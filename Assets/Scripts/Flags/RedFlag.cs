using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlag : Flags
{
    // Start is called before the first frame update
    public GameObject Spawn;
    public GameObject Restriction;
    public EventHandler onRedFlagPickup;

    
    void Awake()
    {
        Restriction = GameObject.FindGameObjectWithTag("Player");
        Restricted = Restriction;
        IsAtBase = true;
        Spawnlocation = Spawn.transform.position;

    }

    private void Start()
    {
        OnflagPickedUp += RedPickup;
    }

  

    
    private void RedPickup(object sender, EventArgs e)
    {
         onRedFlagPickup?.Invoke(this, EventArgs.Empty);
    }

    override 
    public void Respawn()
    {
        gameObject.transform.position = Spawn.transform.position;
        gameObject.transform.rotation = Spawn.transform.rotation;
    }

}
