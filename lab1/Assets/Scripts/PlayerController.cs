using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public KeyCode keyRight;
    public KeyCode keyLeft;
    public AngleController angleController;
    public GameManager gm;
    public GameObject bullet;
    public Transform shotPos;
    public float shotSpace;

    //// current velocity
    private float horizontalVel = 0.0f;
    private float forwardVel = 3.0f;

    public void decVel()
    {
        forwardVel -= 1;
    }

    private float nextShot;
    // 每当时间过去10s，就加速
    private float curTime = 0.0f;

    void Start()
    {
        
        angleController = GameObject.Find("Player").GetComponent<AngleController>();
    }

    void Update()
    {
        // print(transform.position);
        //// control with keyboard
        if (Input.GetKey(keyLeft))
            horizontalVel = -3.0f;
        else if (Input.GetKey(keyRight))
            horizontalVel = +3.0f;
        else
            horizontalVel = 0.0f;
        //// TODO: Your Implementation:
        //// - Update the horizontal velocity with angleController
        horizontalVel = angleController.movingSpeed;

        //// When not dead, update velocity
        if (!GameManager.Instance.IsDead())
        {
            this.transform.GetComponent<Rigidbody>().velocity = new Vector3(horizontalVel, 0.0f, forwardVel);
        }

        if(Time.timeSinceLevelLoad - curTime >= 10)
        {
            curTime += 10;
            if (forwardVel < 8)
                forwardVel += 1;
        }

        if (Input.GetMouseButtonDown(0) && Time.time > nextShot)
        {
            nextShot = Time.time + shotSpace;
            Instantiate(bullet, new Vector3(transform.position.x, 2.0f, transform.position.z), transform.rotation);
        }
    }

    public float curVel()
    {
        return forwardVel;
    }
    
    void OnTriggerEnter(Collider other) {
        //// TODO: Your Implementation:
        //// - When collide with obj with tag 'CollisionWall' or 'FallWall', trigger OnDeath() in GameManager
        if(other.gameObject.tag == "CollisionWall" || other.gameObject.tag == "FallWall")
        {
            print("call OnDeath");
            gm.OnDeath(other);
        }

        if(other.gameObject.tag == "slow")
        {
            if (forwardVel > 1)
                forwardVel -= 1;
        }
    }
}
