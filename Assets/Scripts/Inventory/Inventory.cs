using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public RedFlag redflag;
    public BlueFlag blueflag;
    
    private bool _hasBlueFlag = false;
    private bool _hasRedFlag = false;


    public EventHandler aiFlagEquipt;
    public EventHandler playerFlagEquipt;
    public EventHandler FlagDropped;
    // Start is called before the first frame update
    void Start()
    {
        //subscribes to events
        blueflag.playerOnBlueFlagPickup += playerHasBlueFlag;
        redflag.playerOnRedFlagPickup += playerHasRedFlag;
        blueflag.aiOnBlueFlagPickup += aiHasBlueFlag;
        redflag.aiOnRedFlagPickup += aiHasRedFlag;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    //destroys events once reloading
    private void OnDestroy()
    {
        blueflag.playerOnBlueFlagPickup -= playerHasBlueFlag;
        redflag.playerOnRedFlagPickup -= playerHasRedFlag;
        blueflag.aiOnBlueFlagPickup -= aiHasBlueFlag;
        redflag.aiOnRedFlagPickup -= aiHasRedFlag;
    }

    
    
    //events for when player or AI has flag
    private void playerHasRedFlag(object sender, EventArgs e)
    {
        if (gameObject.tag == "Player")
        {
            _hasRedFlag = true;
            PlayerFlagEquipt();
        }
      
    }
    private void playerHasBlueFlag(object sender, EventArgs e)
    {
        if (gameObject.tag == "Player")
        {
            _hasBlueFlag = true;
            PlayerFlagEquipt();
        }
       
    }
    private void aiHasRedFlag(object sender, EventArgs e)
    {
        if (gameObject.tag == "AI")
        {
            _hasRedFlag = true;
            AiFlagEquipt();
        }
    }
    private void aiHasBlueFlag(object sender, EventArgs e)
    {
        if (gameObject.tag == "AI")
        {
            _hasBlueFlag = true;
            AiFlagEquipt();
        }
        
    }
    
 

    private void AiFlagEquipt()
    {
        aiFlagEquipt?.Invoke(this,EventArgs.Empty);
    } 
    private void PlayerFlagEquipt()
    {

        playerFlagEquipt?.Invoke(this,EventArgs.Empty);
    }
    
    
    private void flagDropped()
    {
        FlagDropped?.Invoke(this,EventArgs.Empty);
    }
    
}
