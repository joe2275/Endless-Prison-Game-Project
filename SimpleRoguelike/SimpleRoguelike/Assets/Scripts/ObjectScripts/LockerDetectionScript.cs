using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerDetectionScript : MonoBehaviour {
    private AudioSource keySound;
    private bool isEntered = false;
    void Awake()
    {
        keySound = GetComponent<AudioSource>();
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isEntered)
        {
            isEntered = true;
            keySound.Play();

            ObjectManagerScript.GetInstance().InstantiateKey();
        }
    }
}
