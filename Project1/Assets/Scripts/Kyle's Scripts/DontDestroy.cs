/*****************************************************************************
// File Name :         DontDestroy.cs
// Author :            Kyle Grenier
// Creation Date :     October 01, 2020
//
// Brief Description : Used for keeping the GameObject which plays the background music alive througout the entire game.
*****************************************************************************/
using UnityEngine;

public class DontDestroy : Singleton<DontDestroy>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}