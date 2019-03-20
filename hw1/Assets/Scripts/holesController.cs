using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class holesController : MonoBehaviour
{
    public int score = 0;
    public int life = 2;
    // Start is called before the first frame update
    void Start()
    {
        life = 2;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if mother ball falls to hole, minus `life` and put it back to the origin position
        if(other.gameObject.name == "Ball_00")
        {
            other.gameObject.transform.position = new Vector3(11.14f, 1.13f, 0);
            // update life
            life--;
            GameObject lifeObj = GameObject.Find("Canvas/LifeText");
            Text lifeText = (Text)lifeObj.GetComponent<Text>();
            lifeText.text = "life:" + life; 
            if (life == 0)
            {
                Application.LoadLevel("Fail");
            }
        }
        else
        {
            // add score
            string num = other.gameObject.name.Substring(5, 2);
            int val = int.Parse(num);
            score += val;
            GameObject scoreObj = GameObject.Find("Canvas/ScoreText");
            Text scoreText = (Text)scoreObj.GetComponent<Text>();
            scoreText.text = "score: " + score;
            // if score is bigger than 20, let user win
            if(score >= 20)
            {
                GameObject winObj = GameObject.Find("Canvas/WinText");
                Text winText = (Text)winObj.GetComponent<Text>();
                winText.text = "Congratulations!";
                Application.LoadLevel("Win");
            }
            // other ball should disappear after falls to holes
            Destroy(other.gameObject);
        }
    }
}
