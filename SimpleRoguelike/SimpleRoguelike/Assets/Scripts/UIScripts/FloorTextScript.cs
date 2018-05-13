using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorTextScript : MonoBehaviour {
    private static int floor = 1;
    //private int floor;
    private Text floorText;
	// Use this for initialization
	void Awake () {
        floorText = GetComponent<Text>();

        if (floor == 1)
        {
            DontDestroyOnLoad(gameObject);
        }

        ShowFloor();
    }

    public void IncreaseFloor()
    {
        floor++;
        ShowFloor();
    }

    private void ShowFloor()
    {
        floorText.text = "Floor : " + floor;
    }

    public void InitFloor()
    {
        floor = 1;
        ShowFloor();
    }

    public int GetFloor()
    {
        return floor;
    }
}
