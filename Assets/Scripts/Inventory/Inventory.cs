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

    public GameObject[] flags;
    // Start is called before the first frame update
    void Start()
    {
        blueflag.OnBlueFlagPickup += hasBlueFlag;
        redflag.onRedFlagPickup += hasRedFlag;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void hasRedFlag(object sender, EventArgs e)
    {
        _hasRedFlag = true;
        Debug.Log("has red flag");
    }
    private void hasBlueFlag(object sender, EventArgs e)
    {
        _hasBlueFlag = true;
    }
    
}
