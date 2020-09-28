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
    //Room dimensions, subject to change
    public float maxX = 4.5f;
    public float minX = -4.5f;
    public float maxZ = 8f;
    public float minZ = -6f;
    public float maxY = 5;

    //array of # of toys in scene, and the max allowed amount
    private GameObject[] numberOfToys;
    private int maxNumberOfToys;
    private bool doneSpawning = false;

    //array of possible toys to spawn
    public GameObject[] typesOfToys;
    private int toyIndex;

    void Start()
    {
        maxNumberOfToys = ScoreManager.instance.WinningScore;
    }

    // counts number of toys in room every frame, if != to max toys, call SpawnToy()
    void Update()
    {
        numberOfToys = GameObject.FindGameObjectsWithTag("Toy");


        if (numberOfToys.Length != maxNumberOfToys && doneSpawning == false)
        {
            SpawnToy();

            if (numberOfToys.Length == maxNumberOfToys - 1) doneSpawning = true;
        }

    }

    //creates a toy at a random spot in the room
    void SpawnToy()
    {
        int toyIndex = Random.Range(0, typesOfToys.Length);

        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));

        Instantiate(typesOfToys[toyIndex], spawnPosition, typesOfToys[toyIndex].transform.rotation);

    }
}
