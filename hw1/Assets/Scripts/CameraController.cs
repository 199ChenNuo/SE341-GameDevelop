using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour
{
    public GameObject sphere;   // white ball
    public float moveSpeed;
    public float sensitivetyMouseWheel = 10f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        GameObject lifeObj = GameObject.Find("Canvas/TimeText");
        Text lifeText = (Text)lifeObj.GetComponent<Text>();
        lifeText.text = "time remain:" + (int)(30- Time.timeSinceLevelLoad) + "s";
        if (sphere.transform.position.y < -0.5)
        {
            Application.LoadLevel("Fail");
        }
        float mouseX = Input.GetAxis("Mouse X") * moveSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * moveSpeed;
        if(transform.position.y>0 && transform.position.y<30)
        {
            transform.RotateAround(sphere.transform.position, new Vector3(0, 0, 1), -mouseY * 1.5f);
        }
        transform.RotateAround(sphere.transform.position, new Vector3(0, 1, 0), mouseX);
        transform.LookAt(sphere.transform.position);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.GetComponent<Camera>().fieldOfView = this.GetComponent<Camera>().fieldOfView - Input.GetAxis("Mouse ScrollWheel") * sensitivetyMouseWheel;
        }
    }
}
