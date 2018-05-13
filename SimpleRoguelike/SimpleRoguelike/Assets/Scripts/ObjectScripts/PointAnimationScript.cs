using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAnimationScript : MonoBehaviour {
    public Sprite[] pointSprite;

    private SpriteRenderer render;

    private int count = 0;
    private bool isPlayable = true;

    const float WAIT_LONG_TIME = 2f;
    const float WAIT_SHORT_TIME = 0.1f;

	// Use this for initialization
	void Start () {
        render = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        PlayAnimation();
	}

    private void PlayAnimation()
    {
        if(isPlayable)
        {
            if (count != pointSprite.Length)
            {
                render.sprite = pointSprite[count++];
                StartCoroutine(WaitForShortTime());
            }
            else
            {
                render.sprite = null;
                count = (count + 1) % (pointSprite.Length + 1);
                StartCoroutine(WaitForLongTime());
            }
        }
    }

    private IEnumerator WaitForShortTime()
    {
        isPlayable = false;
        yield return new WaitForSeconds(WAIT_SHORT_TIME);
        isPlayable = true;
    }

    private IEnumerator WaitForLongTime()
    {
        isPlayable = false;
        yield return new WaitForSeconds(WAIT_LONG_TIME);
        isPlayable = true;
    }
}
