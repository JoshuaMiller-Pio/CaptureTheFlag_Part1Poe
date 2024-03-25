using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlag : Flags
{
    // Start is called before the first frame update
    public GameObject Spawn;
    public GameObject Restriction;
    public EventHandler playerOnRedFlagPickup;
    public EventHandler aiOnRedFlagPickup;
   

    
    void Awake()
    {
        Restriction = GameObject.FindGameObjectWithTag("Player");
        PC = Restriction.GetComponent<PlayerController>();
        ai = GameObject.FindGameObjectWithTag("AI").GetComponent<AIController>();
        Restricted = Restriction;
        IsAtBase = true;
        Spawnlocation = Spawn.transform;

    }

    private void Start()
    {
        _collider = gameObject.GetComponent<SphereCollider>();
        OnflagPickedUp += RedPickup;

    }

  

    
    private void RedPickup(object sender, EventArgs e)
    {
        if (Holder != null)
        {
            if (Holder.tag == "Player" )
            {
                playerOnRedFlagPickup?.Invoke(this, EventArgs.Empty);
            
            }
            else
            {
                aiOnRedFlagPickup?.Invoke(this, EventArgs.Empty);
            
            }
        }
      
    }

    override 
    public void Respawn()
    {
        Holder = null;

        gameObject.transform.position = Spawn.transform.position;
        gameObject.transform.rotation = Spawn.transform.rotation;
        IsAtBase = true;

    }

}
