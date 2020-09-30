/* Frank Calabrese 
 * Project 2
 * spawns toys randomly within given coordinates
 */

using UnityEngine;

public class ToySpawner : Singleton<ToySpawner>
{ 
    //Room dimensions, subject to change
    public float maxX = 4.5f;
    public float minX = -4.5f;
    public float maxZ = 8f;
    public float minZ = -6f;
    public float maxY = 5;

    //Current # of toys in scene, and the max allowed amount
    private int numberOfToys;
    private int maxNumberOfToys;
    private GameObject[] countToys;

    //array of possible toys to spawn
    public GameObject[] typesOfToys;
    private int toyIndex;

    //Spawns a toy at a random spot in the room
    private void SpawnToy()
    {
        int toyIndex = Random.Range(0, typesOfToys.Length);

        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), maxY, Random.Range(minZ, maxZ));

        Instantiate(typesOfToys[toyIndex], spawnPosition, typesOfToys[toyIndex].transform.rotation);

    }

    //Spawn as much toys as designated in the ScoreManager.
    public void SpawnToys()
    {
        maxNumberOfToys = ScoreManager.instance.WinningScore;

        for (int i = 0; i < maxNumberOfToys; ++i)
        {

            SpawnToy();
        }

        CheckToys();
    }

    public void CheckToys()
    {
       countToys = GameObject.FindGameObjectsWithTag("Toy");

        for (int i = countToys.Length -1; i < maxNumberOfToys; ++i)
        {
            SpawnToy();
            ScoreManager.instance.Score--;
        }
    }
}