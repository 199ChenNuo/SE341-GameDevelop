using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        Application.LoadLevel("Game");
    }


    public void exitGame()
    {
        Application.Quit();
    }
}
