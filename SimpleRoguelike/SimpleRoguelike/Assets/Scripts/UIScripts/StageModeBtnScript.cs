using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageModeBtnScript : AButtonScript {

    private new void Start()
    {
        base.Start();

    }


    public override void OnClickLoadScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
