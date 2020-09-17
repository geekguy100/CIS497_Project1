/* Frank Calabrese 
 * Project 2
 * spawns toys randomly within given coordinates
 * replaces destroyed toys
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawner : MonoBehaviour
{

    public GameObject toy;

    //Room dimensions, subject to change
    public float maxX = 4.5f;
    public float minX = -4.5f;
    public float maxZ = 8f;
    public float minZ = -6f;
    public float maxY = 5;

    //array of # of toys in scene, and the max allowed amount
    private GameObject[] numberOfToys;
    public int maxNumberOfToys = 5;

    //array of possible toy prefabs to spawn
    //public GameObject[] typesOfToys;
    //private int toyIndex;

    void Start()
    {
        
    }

    // counts number of toys in room every frame, if != to max toys, call SpawnToy()
    void Update()
    {

        numberOfToys = GameObject.FindGameObjectsWithTag("Toy");

        if (numberOfToys.Length != maxNumberOfToys)
        {
            //int toyIndex = Random.Range(0, typesOfToys.Length);
            SpawnToy();
        }
    }

    //creates a toy at a random spot in the room
    void SpawnToy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));

        Instantiate(toy, spawnPosition, toy.transform.rotation);

    }
}
