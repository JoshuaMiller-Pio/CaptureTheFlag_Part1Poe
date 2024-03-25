using System;
using System.Collections;
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
    public Canvas win, loss;
    public EventHandler RestartRound, StartGame, GameOver;
    public bool blueatBase = true;
    public bool Paused
    {
        get => _paused;
        set => _paused = value;
    }

    public void GameStart()
    {
        //starts the count down for the game
        StartCoroutine(startGame());
    }

    // Start is called before the first frame update
    void Start()
    {
        //locks and hide cursor for fps feel
        
        updateUI();
        
    }



//checks the win loss conditoons or restarts the round
private void WinLose()
    {
        if (Ppoints >= 5)
        {
            win.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            GameOver?.Invoke(this,EventArgs.Empty);
        }
        else if (Apoints >= 5)
        {
            
            loss.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            GameOver?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            Restart();
        }
       
     
        
    }

//sets player points
    public void setPpoints()
    {
        Ppoints++;
        updateUI();
        WinLose();


    }
    //sets AI points
    public void setApoints()
    {

        Apoints++;
        updateUI();
        WinLose();

    }
    //updates the UI with the new score
    public void updateUI()
    {
        Blue.text = Ppoints.ToString();
        Red.text = Apoints.ToString();
    }


    
    #region GameControls
        
    
        public void OnPause()
        {
            freezeTime();
        }
        //freezes game when paused
        private void freezeTime()
        {
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
                return;
            }

            Time.timeScale = 1;
        }
        //button controls for play
        public void Play()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SceneManager.LoadScene(1);
        }
        
        //button controls for restart
        public void Restart()
        {
            RestartRound?.Invoke(this, EventArgs.Empty);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        }
        
        //full reset of the game including score if players want to play again
        public void ResetGame()
        {
            Ppoints = 0;
            Apoints = 0;
            Cursor.visible = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        }
        
        //quits game
        public void Quit()
        {
            Application.Quit();

        }

    #endregion

    //after alloted time game starts
    IEnumerator startGame()
    {
        yield return new WaitForSeconds(3);
        SoundManager.Instance.playStart();
        StartGame?.Invoke(this,EventArgs.Empty);
        _paused = false;
        yield return null;
    }
}
