using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoombaAIScript : MonoBehaviour
{
    private NavMeshAgent navMesh;
    private NavMeshPath path;
    private bool inCoRoutine = false;
    private Vector3 target;

    public float timeforNewPath;
    public int xMax, xMin;
    public int zMax, Zmin;

    // Start is called before the first frame update
    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();

        path = new NavMeshPath();
    }

    private void Update()
    {
        if(!inCoRoutine)
        {
            waitForNewPos();
        }
    }

    private Vector3 GetNewRandomPos()
    {
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(Zmin, zMax);

        Vector3 pos = new Vector3(x, 0, z);

        return pos;
    }

    IEnumerator waitForNewPos()
    {
        inCoRoutine = true;
        yield return new WaitForSeconds(timeforNewPath);
        GetNewPath();

        if(!navMesh.CalculatePath(target,path))
        {
            Debug.Log("Invalid Path");
        }

        inCoRoutine = false;
    }

    private void GetNewPath()
    {
        target = GetNewRandomPos();
        navMesh.SetDestination(target);
    }
}
