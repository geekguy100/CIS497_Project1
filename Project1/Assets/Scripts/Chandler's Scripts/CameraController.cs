using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;

    public float sensitivity = 500f;
    public Transform playerBody;

    private void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("Mouse Sensitivity", sensitivity);
        if (sensitivity == 0f)
        {
            sensitivity = 500f;
            PlayerPrefs.SetFloat("Mouse Sensitivity", sensitivity);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Don't run if the game hasn't started or has ended.
        if (!GameManager.instance.gameStarted || GameManager.instance.GameOver)
            return;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX * sensitivity * Time.deltaTime);
    }
}
