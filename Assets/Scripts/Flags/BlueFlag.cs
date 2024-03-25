using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlag : Flags
{
    public  GameObject Restriction;
    public GameObject Spawn;
    public event EventHandler aiOnBlueFlagPickup;
    public event EventHandler playerOnBlueFlagPickup;
    //set up above and for red
    void Awake()
    {

        Restriction = GameObject.FindGameObjectWithTag("AI");
        ai = Restriction.GetComponent<AIController>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    
        Restricted = Restriction;
        IsAtBase = true;
        Spawnlocation = Spawn.transform;


    }

    private void Start()
    {
        _collider = gameObject.GetComponent<SphereCollider>();
        OnflagPickedUp += BluePickup;
        

    }

 

   
    private void BluePickup(object sender, EventArgs e)
    {
        if (Holder != null)
        {
            GameManager.Instance.blueatBase = IsAtBase;
            if (Holder.tag == "Player" )
            {
                playerOnBlueFlagPickup?.Invoke(this, EventArgs.Empty);
            
            }
            else
            {
                aiOnBlueFlagPickup?.Invoke(this, EventArgs.Empty);
            
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
        GameManager.Instance.blueatBase = IsAtBase;
        
    }
  
}
