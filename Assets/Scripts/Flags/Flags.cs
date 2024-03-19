using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flags : MonoBehaviour
{
    #region Declarations

        public event EventHandler OnflagPickedUp;

        private bool _isPickedup = false;
        private bool _isAtBase = true;
        private Transform _spawnLocation;
        private GameObject _restricted,_holder=null;
        public PlayerController PC;
        public AIController ai;

    #endregion
    
    #region Accessors & Mutators

    

    
        public bool IsPickedUp
        {
            get => _isPickedup;
            set => _isPickedup = value;
        }
        public GameObject Holder
        {
            get => _holder;
            set => _holder = value;
        }
        public GameObject Restricted
        {
            get => _restricted;
            set => _restricted = value;
        }
        
        public bool IsAtBase
        {
            get => _isAtBase;
            set => _isAtBase = value;
        }
        public Transform Spawnlocation
        {
            get => _spawnLocation;
            set => _spawnLocation = value;
        }
        public Flags()
        {
          
            
        }
        public Flags(bool isAtBase, GameObject restricted, Transform spawnlocation)
        {
            Spawnlocation = spawnlocation;
            Restricted = restricted;
            IsAtBase = isAtBase;
        }
#endregion


private void Start()
{
    GameManager.Instance.RestartRound += RestartRound;
}


public void RestartRound(object sender, EventArgs e)
{
    Respawn();
}


private void Update()
    {

        if (_isPickedup)
        {

            gameObject.transform.position = new Vector3(_holder.transform.position.x,_holder.transform.position.y,_holder.transform.position.z);
            gameObject.transform.rotation = _holder.transform.rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {


        //pickup at base
        if (_isAtBase && other.gameObject != Restricted && other.gameObject.tag != "Blue_Base" &&
            other.gameObject.tag != "Red_Base" && other.gameObject.tag != "bullet")
        {
            _isAtBase = false;
            _holder = other.gameObject;
            _isPickedup = true;

            //fires flag pick up event
            OnflagPickedUp?.Invoke(this, EventArgs.Empty);
            assignDroppoint();

        }
        //pickup in the field 
        else if(!_isAtBase && !_isPickedup && (other.tag == "Player" || other.tag == "AI") && (other.gameObject.tag != "Blue_Base" || other.gameObject.tag != "Red_Base") && other.gameObject.tag != "bullet")
        {
            _isPickedup = true;
            OnflagPickedUp?.Invoke(this, EventArgs.Empty);
            _holder = other.gameObject;
            assignDroppoint();


        }
        
        
        void assignDroppoint()
        {
            
                Debug.Log("dropped assigned");
            if (_holder.tag == "Player" )
            {
                PC.Flagdropped += drop;
                Debug.Log("subbed player");
            }
            else if (_holder.tag == "AI" )
            {
                ai.Flagdropped += drop;
                Debug.Log("subbed AI");
                
            }
        }
        

        //if there is a holder then a point can be awarded
        if (_holder!= null)
        {
            if (other.tag == "Red_Base" && _holder.tag == "AI" && gameObject.tag == "Flag_Red")
            {
                _isPickedup = false;
                GameManager.Instance.setApoints();
                Respawn();
            }
            else if(other.tag == "Red_Base"&& _holder.tag == "AI" && gameObject.tag == "Flag_Blue")
            {
                _isPickedup = false;

                Respawn();
            }
            if (other.tag == "Blue_Base" && _holder.tag == "Player" && gameObject.tag == "Flag_Blue")
            {
                _isPickedup = false;
                Respawn();
                GameManager.Instance.setPpoints();
            }
            else if(other.tag == "Blue_Base"&& _holder.tag == "Player" && gameObject.tag == "Flag_Red")
            {
                _isPickedup = false;

                Respawn();
            }
        }
       
     
    }

    public void drop(object sender, EventArgs e)
    {
        _isPickedup = false;
        transform.position = _holder.transform.position;
        transform.rotation = _spawnLocation.rotation;

    }

    public abstract void Respawn();


}
