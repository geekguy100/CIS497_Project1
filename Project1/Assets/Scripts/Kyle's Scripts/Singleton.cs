/*****************************************************************************
// File Name :         Singleton.cs
// Author :            Buttfield-Addison, Manning, Nugent
// Creation Date :     09/04/2020
//
// Brief Description : A class that enables a script to have a singleton behaviour. 
                       Such a script should derive from Singleton<T> instead of MonoBehaviour.
                       
                       This class has been modified from OriginalSingleton.cs to make sure any preexisting instances
                       of the GameObject deriving from this class are destroyed, ensuring there is only ONE instance in existance.
*****************************************************************************/
using UnityEngine;

//Singleton class taken from 'Unity Game Development Cookbook' (Buttfield-Addison, Manning, Nugent)
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool isEnabled { get; private set; }

    public static T instance{
        get {
            //If we don't have an instance ready, get one by either finding it in the scene or creating one.
            if (_instance == null)
                _instance = FindOrCreateInstance();

            return _instance;
        }
    }

    //NOTE: I, Kyle Grenier, added this function in.
    //I noticed that if I were to add in multiple GameManagers, for example,
    //everything related to the GameManager would call multiple times, depending on the number of clones.
    //To prevent that, we need to make sure we remove any pre-existing instances of our Singleton<T>.
    //Afterall, the design premise around Singletons is to have only ONE instance.
    private void OnEnable()
    {
        //Check to see if there are already multiple instances.
        //If so, destroy all but one.
        T[] clones = FindObjectsOfType<T>();

        for (int i = 0; i < clones.Length; ++i)
        {
            //If a gameObject of type T is found and is enabled, meaning it is the original, 
            //destroy this game object because it's a clone.
            if (clones[i].GetComponent<Singleton<T>>().isEnabled)
                Destroy(gameObject);
        }

        //No game objects of type T were found to be enabled, so make yourself the original.
        isEnabled = true;
    }

    //This variable is the actual instance. 
    //It's private, and can only be accessed through the 'instance' property above.
    private static T _instance;

    //Attemps to find an instance of this singleton. If one can't be found, a new one is created.
    private static T FindOrCreateInstance()
    {
        //Attempt to locate an instance.
        var instance = FindObjectOfType<T>();

        if (instance != null)
        {
            //We found an instance. Return it; it will be used as the shared instance.
            return instance;
        }

        //If an instance hasn't been found, set up a new one.
        //We'll need to create a new game object to hold this instance.
        var name = typeof(T).Name + " Singleton";

        //Create the container gameobject with that name.
        var containerGameObject = new GameObject(name);

        //Create and attach a new instance of whatever 'T' is; we'll return this new instance.
        var singletonComponent = containerGameObject.AddComponent<T>();

        return singletonComponent;
    }
}