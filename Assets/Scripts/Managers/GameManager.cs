using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : Singleton<GameManager>
{
    private int Ppoints = 0, Apoints = 0;
    public TextMeshProUGUI Blue = null, Red = null;
    private bool _paused = true;
    public EventHandler RestartRound, StartGame;
    public bool blueatBase = true;
    public bool Paused
    {
        get => _paused;
        set => _paused = value;
    }

    public void GameStart()
    {
        StartCoroutine(startGame());
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        Restart();
        Ppoints++;
        updateUI();
    }
    public void setApoints()
    {
        Restart();
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

        public void OnPause()
        {
            freezeTime();
        }

        private void freezeTime()
        {
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
                return;
            }

            Time.timeScale = 1;
        }
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

    IEnumerator startGame()
    {
        yield return new WaitForSeconds(3);
        SoundManager.Instance.playStart();
        StartGame?.Invoke(this,EventArgs.Empty);
        Debug.Log("called");
        _paused = false;
        yield return null;
    }
}
