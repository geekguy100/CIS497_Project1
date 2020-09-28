/*Frank Calabrese  
 * Increments score in ScoreManager script 
 * when toy is thrown in toybox
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBoxCollider : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Toy"))
        {
            ScoreManager.instance.Score++;

            Destroy(other.gameObject);
        }

        if (other.CompareTag("GoldenToy"))
        {
            ScoreManager.instance.Score+=2;

            Destroy(other.gameObject);
        }
    }
}
