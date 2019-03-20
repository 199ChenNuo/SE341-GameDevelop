using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileBallControoller : MonoBehaviour
{
    public int speed;
    public GameObject mainCamera;
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
        if (Input.GetMouseButtonUp(0))
        {
            // add a force to mother ball when user click the mouse
            rb.AddForce((rb.transform.position - new Vector3(mainCamera.transform.position.x, 1.13f, mainCamera.transform.position.z)) * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Substring(0, 4) == "Ball")
        {
            // play the audio when mother ball hits other ball(s)
            audio.Play();
        }
    }
}
