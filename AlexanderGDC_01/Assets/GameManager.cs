using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public int scoreVal;
    public int defaultScore;
    public TMP_Text scoreTB;
    public TMP_Text scoreValTB;

    public int livesVal;
    public int defaultLives;
    public TMP_Text livesTB;
    public TMP_Text livesValTB;

    public int timerVal;
    public string currentTime;
    public TMP_Text timerTB;
    public TMP_Text timerValTB;

    public bool beatLvlCheck;
    public bool timedLvlCheck;
    public bool bkMusicCheck;
    public bool bkMusicOver;
    public bool playerDead;
    public bool isOver;
    public bool gameStart;

    public AudioSource bkMusic;
    public AudioSource gameOver;
    public AudioSource beatLvl;
    public AudioSource source;

    public GameObject player;
    public GameObject MenuCanvas;
    public GameObject HUDCanvas;
    public GameObject EndScreenCanvas;
    public GameObject FooterCanvas;

    public string endMsgTxt;
    public TMP_Text endMsgTB;

    public string gameOverTxt;
    public string winTxt;
    public string loseTxt;
    public TMP_Text gameOverTB;

    public string titleTxt;
    public TMP_Text titleTB;

    public string creditsTxt;
    public TMP_Text creditsTB;

    public string copyrightTxt;
    public string copyYear;  //System.DateTime.Now.ToString("yyyy")
    public TMP_Text copyrightTB;

    public string firstLvl;
    public string nextLvl;
    public string loadLvl;
    public string currentLvl;

    public enum GameState { PLAYING, DEATH, GAMEOVER, BEATLEVEL}
    private GameState myState;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        HideMenus();
        loadLvl = currentLvl;
        PlayGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
            Quit();

    }

    void MainMenu()
    {
        defaultLives = 3;
        defaultScore = 0;
        titleTxt = "Game Title";
        creditsTxt = "Credits";
        copyrightTxt = "CopyRight";
        if (MenuCanvas != null)
            MenuCanvas.gameObject.SetActive(true);
        if (FooterCanvas != null)
            FooterCanvas.gameObject.SetActive(true);
    }

    void Quit()
    {
        Application.Quit();
        scoreVal = defaultScore;
        if (scoreVal != null)
        {
            scoreValTB.text = "" + scoreVal + "";
            scoreTB.text = "Score: " + scoreVal;
        }
        livesVal = defaultLives;
        if (livesVal != null)
        {
            livesValTB.text = "" + livesVal + "";
            livesTB.text = "Score: " + livesVal;
        }

        switch(myState)
        {
            case GameState.PLAYING:
                if (playerDead)
                {
                    if (livesVal > 0)
                    {
                        livesVal -= 1;
                        Reset();
                    }
                    if (beatLvlCheck)
                        myState = GameState.BEATLEVEL;
                    if (timedLvlCheck)
                    {
                        if (timerVal < 0)
                            myState = GameState.GAMEOVER;
                        else
                        {
                            currentTime = Time.time.ToString();
                            timerVal = int.Parse(currentTime);
                            timerTB.text = "Time: " + timerVal;
                        }
                    }    
                }
                break;
            case GameState.DEATH:
                if (bkMusicCheck)
                {
                    bkMusic.volume = 0;
                    if (bkMusic.volume == 0)
                        bkMusicOver = true;
                    if(bkMusicOver || bkMusic == null)
                    {
                        if (gameOver != null)
                            source = gameOver;
                        gameOverTxt = loseTxt;
                        myState = GameState.GAMEOVER;

                    }
                }
                break;
            case GameState.BEATLEVEL:
                if (bkMusicCheck)
                {
                    bkMusic.volume = 0;
                    if (bkMusic.volume == 0)
                        bkMusicOver = true;
                    if (bkMusicOver || bkMusic == null)
                    {
                        if (beatLvl != null)
                            source = beatLvl;
                        if (nextLvl != null)
                            StartNextLevel();
                        else
                        {
                            gameOverTxt = winTxt;
                            myState = GameState.GAMEOVER;
                        }

                    }
                }
                break;
            case GameState.GAMEOVER:
                player.gameObject.SetActive(false);
                HideMenus();
                if(EndScreenCanvas != null)
                {
                    EndScreenCanvas.gameObject.SetActive(true);
                    gameOverTB.text = "Game Over";
                }
                break;
        }
    }

    void PlayGame()
    {
        HideMenus();
        if (HUDCanvas != null)
            HUDCanvas.gameObject.SetActive(true);
        if (timedLvlCheck)
        {
            currentTime = Time.time.ToString();
            timerVal = int.Parse(currentTime); 
            timerTB.text = "Time: " + timerVal;
        }
        scoreVal = defaultScore;
        if(scoreVal != null)
        {
            scoreValTB.text = "" + scoreVal + "";
            scoreTB.text = "Score: " + scoreVal;
        }
        livesVal = defaultLives;
        if (livesVal != null)
        {
            livesValTB.text = "" + livesVal + "";
            livesTB.text = "Score: " + livesVal;
        }

        gameStart = true;
        myState = GameState.PLAYING;
        playerDead = false;
        SceneManager.LoadScene(loadLvl, LoadSceneMode.Additive);
        currentLvl = loadLvl;
    }

    void ResetLevel()
    {
        playerDead = false;
        SceneManager.UnloadSceneAsync(currentLvl);
        loadLvl = firstLvl;
        PlayGame();
    }

    void StartNextLevel()
    {
        bkMusicOver = false;
        scoreVal = defaultScore;
        livesVal = defaultLives;
        SceneManager.UnloadSceneAsync(currentLvl);
        loadLvl = nextLvl;
        PlayGame();
    }

    void RestartGame()
    {
        scoreVal = defaultScore;
        livesVal = defaultLives;
        SceneManager.UnloadSceneAsync(currentLvl);
        loadLvl = firstLvl;
        PlayGame();
    }

    //reset in-component variables
    void Reset()
    {
        
    }

    //hide canvases
    void HideMenus()
    {
        MenuCanvas.gameObject.SetActive(false);
        HUDCanvas.gameObject.SetActive(false);
        EndScreenCanvas.gameObject.SetActive(false);
        FooterCanvas.gameObject.SetActive(false);
    }
}
