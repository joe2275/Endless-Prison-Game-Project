using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerScript : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && PlayerManagerScript.GetInstance().GetPlayerScript().HasKey())
        {
            Destroy(gameObject);
        }
    }
}
