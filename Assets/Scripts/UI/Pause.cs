using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] turnoff ,turnOn;
    private bool active = false;
    private GameObject player;
    
    void Start()
    {
        GameManager.Instance.GameStart();

        player = GameObject.FindWithTag("Player");
        GameManager.Instance.StartGame += startGame;
        player.GetComponent<PlayerController>().enabled = false;
   
    }

   private void startGame(object sender, EventArgs e)
    {
        player.GetComponent<PlayerController>().enabled = true;

    }

   private void OnDestroy()
   {

   }

   // Update is called once per frame
    void Update()
    {
        //pressing esc pauses the game and shows the appropriate panel 
        if (Input.GetButtonDown("Cancel"))
        {
            
            GameManager.Instance.OnPause();
            
            
            if (active == false)
            {
                for (int i = 0; i < turnoff.Length; i++)
                {
                    turnoff[i].SetActive(false);
                }
                for (int i = 0; i < turnOn.Length; i++)
                {
                    turnOn[i].SetActive(true);
                }

                active = true;
                Screen.lockCursor = false;
                GameManager.Instance.Paused = active;

                player.GetComponent<PlayerController>().enabled = false;
                player.GetComponent<PlayerLook>().enabled = false;

            }
            else
            {
                for (int i = 0; i < turnoff.Length; i++)
                {
                    turnoff[i].SetActive(true);
                }
                for (int i = 0; i < turnOn.Length; i++)
                {
                    turnOn[i].SetActive(false);
                }

                active = false; 
                Screen.lockCursor = true;
                GameManager.Instance.Paused = active;
                player.GetComponent<PlayerController>().enabled = true;
                player.GetComponent<PlayerLook>().enabled = true;
                


            }
            
            
        }
    }
}
