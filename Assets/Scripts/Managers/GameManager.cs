using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int Ppoints = 0, Apoints = 0;
    public TextMeshProUGUI Blue = null, Red = null;
    private bool _paused, _gameStart;
    public EventHandler RestartRound;
    public bool Paused
    {
        get => _paused;
        set => _paused = value;
    }

    public bool GameStart
    {
        get => _gameStart;
        set => _gameStart = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.lockCursor = true;
        Debug.Log(Ppoints);
        updateUI();
        
    }


    // Update is called once per frame


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
        else
        {
            RestartRound?.Invoke(this, EventArgs.Empty);
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



    #region GameControls

        
        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }

        public void Quit()
        {
            Application.Quit();

        }

    #endregion
}
