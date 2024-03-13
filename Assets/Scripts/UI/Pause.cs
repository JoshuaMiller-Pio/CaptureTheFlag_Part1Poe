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
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
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
