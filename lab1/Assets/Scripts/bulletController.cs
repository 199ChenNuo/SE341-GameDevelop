using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float flySpeed;
    TileManager tileManager;
    private Transform endPos;
    private bool hit = false;
    private float life;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rgb = GetComponent<Rigidbody>();
        rgb.velocity = transform.forward * flySpeed;
        
        life = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rgb = GetComponent<Rigidbody>();
        if (hit)
        {
            rgb.position = endPos.position;
        }
        else
        {
            if (rgb.position.y >= 0.5)
                rgb.position = new Vector3(rgb.position.x, rgb.position.y - 0.01f, rgb.position.z);
            else
                rgb.position = new Vector3(rgb.position.x, 0.5f, rgb.position.z);
        }

         // if (Time.time - life > 10)
         //    gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CollisionWall" || other.gameObject.tag == "FallWall")
        {
            endPos = other.gameObject.transform;
            hit = true;
            Rigidbody rgb = GetComponent<Rigidbody>();
            rgb.velocity = new Vector3(0, 0, 0);
            // other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
