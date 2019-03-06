using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    public float speed = 10.0f;
    public Text countText;
    public Text winText;

   void Start(){
      rb = GetComponent<Rigidbody>();
        count = 0;
        countText.text = "Score: " + count;
        winText.text = "";
   }

   void FixedUpdate(){
        // x and y, but y is considered as '0'
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0, z);

        rb.AddForce(movement * speed);
   }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        count++;
        countText.text = "Score: " + count;
        if (count == 4)
            winText.text = "You Win!!!";
    }
}
