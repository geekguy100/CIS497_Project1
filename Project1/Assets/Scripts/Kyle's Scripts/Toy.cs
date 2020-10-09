/*****************************************************************************
// File Name :         Toy.cs
// Author :            Kyle Grenier
// Creation Date :     September 28, 2020
// Assignment:         Project 2 - CIS 497
// Brief Description : A small script that determines whether or not a toy can be picked up.
                       Can be picked up if it has touched the ground after being thrown recently.
*****************************************************************************/
using UnityEngine;

public class Toy : MonoBehaviour
{
    public bool grabbable = true;

    public void OnToyDrop()
    {
        Invoke("ResetGrabState", 0.5f);
    }

    private void ResetGrabState()
    {
        grabbable = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall") && !grabbable)
            grabbable = true;
    }
}