using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    
	void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 키를 줍는 경우
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManagerScript.GetInstance().GetPlayerScript().SetKey();
            Destroy(gameObject);
        }
    }
}
