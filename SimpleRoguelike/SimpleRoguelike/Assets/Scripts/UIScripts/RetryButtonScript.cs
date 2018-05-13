using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonScript : MonoBehaviour {

	public void ReloadScene()
    {
        PlayerManagerScript.GetInstance().GetFloorTextScript().InitFloor();
        SceneManager.LoadScene("PlayScene");
    }
}
