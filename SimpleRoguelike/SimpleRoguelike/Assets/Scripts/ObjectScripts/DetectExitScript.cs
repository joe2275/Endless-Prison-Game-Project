using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectExitScript : MonoBehaviour {
    private AudioSource windySound;

	// Use this for initialization
	void Start () {
        windySound = GetComponent<AudioSource>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            windySound.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            windySound.Stop();
        }
    }
}
