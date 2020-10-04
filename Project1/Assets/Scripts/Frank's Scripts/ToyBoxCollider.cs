/*Frank Calabrese, Chris Smith  
 * Project 2
 * Increments score in ScoreManager script 
 * when toy is thrown in toybox
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBoxCollider : MonoBehaviour
{
    //Score Sounds
    private AudioSource playerAudio;
    public AudioClip scoreSFX;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

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
        playerAudio.PlayOneShot(scoreSFX, 1.0f);
    }
}
