/*****************************************************************************
// File Name :         MoveCamera.cs
// Author :            Kyle Grenier
// Creation Date :     9/16/2020
// Assignment:         Group Prototype
//
// Brief Description : Moves camera in coordination with mouse input.
*****************************************************************************/
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float horizontalRotate = Input.GetAxisRaw("Mouse X");
        float verticalRotate = Input.GetAxis("Mouse Y");

        transform.RotateAround(transform.position, -Vector3.up, horizontalRotate * horizontalSensitivity);
        transform.RotateAround(transform.position, transform.right, verticalRotate * verticalSensitivity);
    }
}