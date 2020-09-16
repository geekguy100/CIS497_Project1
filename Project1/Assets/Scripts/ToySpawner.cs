using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawner : MonoBehaviour
{

    public GameObject toy;

    //Room dimensions, subject to change
    private float maxX = 4.5f;
    private float minX = -4.5f;
    private float maxZ = 8f;
    private float minZ = -6f;
    private float maxY = 5;

    int numberOfToys = 0;
    int maxNumberOfToys = 5;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(numberOfToys != maxNumberOfToys)
        {
            SpawnToy();
        }
    }

    void SpawnToy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));

        Instantiate(toy, spawnPosition, toy.transform.rotation);

        numberOfToys++;
    }
}
