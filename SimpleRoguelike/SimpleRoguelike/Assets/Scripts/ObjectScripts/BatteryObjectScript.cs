using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryObjectScript : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerManagerScript.GetInstance().GetPlayerScript().GetBattery();
            GameObject.Destroy(gameObject);
        }
    }
}
