using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GameObject.Find("SoundController/Audio Source").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > 30)
        {
            Application.LoadLevel("Fail");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Substring(0, 4) == "Ball")
        {
            // audio.Play();
        }
    }
}
