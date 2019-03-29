using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class GameManager : UnitySingleton<GameManager>
{
    public GameObject player;
    public PlayerController playerController;
    public DeathMenu deathMenu;
    public GameObject scoreObj;
    public TileManager tileManager;
  
    private Text scoreText;
    private float TimeSpend;

    private bool isDead;
    private float highestScore;
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isDead = false;
        scoreObj = GameObject.Find("Canvas/CurrentScore/Text");
        scoreText = (Text)scoreObj.GetComponent<Text>();
        scoreText.text = "current score: 0\ncurrent speed: 3";

        TimeSpend = 0.0f;
        highestScore = PlayerPrefs.GetFloat("highestScore");

        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!isDead) {
            //// TODO: Your Implementation:
            //// 1. update score (Hint: you can use running time as the score)

            //// 2. show score (Hint: show in Canvas/CurrentScore/Text)
            TimeSpend += Time.deltaTime;
            float currentVel = playerController.curVel();
            scoreText.text = "current score: " + (int)TimeSpend + "\ncurrent speed: " + (int)currentVel; 
        }
        else if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) {
            Restart();
        }
    }

    public bool IsDead() {
        return isDead;
    }

    public void OnDeath(bool collision){
        // print("GameOver");
        //// TODO: Your Implementation:
        //// 1. show DeathMenu (Hint: you can use Show() in DeathMenu.cs)
        //// 2. stop player
        //// 3. hide all tiles (Hint: call function in TileManager.cs)
        //// 4. record high score (Hint: use PlayerPrefs)
        highestScore = highestScore > TimeSpend ? highestScore : TimeSpend;
        PlayerPrefs.SetFloat("highestScore", highestScore);
        PlayerPrefs.Save();
        deathMenu.Show(TimeSpend, highestScore);
       
        // stop player
        // PlayerController pc = player.GetComponent<PlayerController>();
        isDead = true;
        tileManager.hideAll();
        // deathMenu.Show(TimeSpend);
    }

    public void Restart(){
        TimeSpend = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
