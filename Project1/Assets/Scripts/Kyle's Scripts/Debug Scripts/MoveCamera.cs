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
    [SerializeField] private bool invertPitch = true;
    
    [SerializeField] private float horizontalSensitivity = 1f;
    [SerializeField] private float verticalSensitivity = 1f;

    private Quaternion initialBodyRotation;
    private Quaternion initialHeadRotation;

    private float pitch;    //Head
    private float yaw;      //Body

    [SerializeField] private Transform head;
    [SerializeField] private float minHeadRotation = -80f;
    [SerializeField] private float maxHeadRotation = 80f;

    private void Awake()
    {
        head = transform.GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        initialBodyRotation = transform.rotation;
        initialHeadRotation = head.rotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Fixed update to allow physics interaction.
    private void FixedUpdate()
    {
        float horizontalRotate = Input.GetAxis("Mouse X") * Time.deltaTime * horizontalSensitivity;
        float verticalRotate = Input.GetAxis("Mouse Y") * Time.deltaTime * verticalSensitivity;

        //Kyle Grenier (10/7): Clamping mouse input between (-1,1) and mulitplying it by the sensitivity to prevent WebGL issues.
        horizontalRotate = Mathf.Clamp(horizontalRotate, -1, 1);
        verticalRotate = Mathf.Clamp(verticalRotate, -1, 1);

        if (invertPitch)
            pitch -= verticalRotate;
        else
            pitch += verticalRotate;

        yaw += horizontalRotate;

        pitch = Mathf.Clamp(pitch, minHeadRotation, maxHeadRotation);

        Quaternion bodyRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        Quaternion headRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        transform.localRotation = initialBodyRotation * bodyRotation;
        head.localRotation = initialHeadRotation * headRotation;
    }
}