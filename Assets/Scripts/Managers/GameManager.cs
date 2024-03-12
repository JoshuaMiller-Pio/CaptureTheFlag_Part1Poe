using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int Ppoints = 0, Apoints = 0;
    public TextMeshProUGUI Blue = null,Red = null;

    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void WinLose()
    {
        if (Ppoints >= 10)
        {
            Debug.Log("PlayerWins");
        }
        else if (Apoints >= 10)
        {
            
            Debug.Log("AIWins");
        }
        
    }

    public void setPpoints()
    {
        Ppoints++;
        updateUI();
    }
    public void setApoints()
    {
        Apoints++;
        updateUI();
    }
    public void updateUI()
    {
        Blue.text = Ppoints.ToString();
        Red.text = Apoints.ToString();
        WinLose();
    }
}
