using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class AButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite unselected;
    public Sprite selected;

    private Image btnSprite;

	protected virtual void Start () {
        btnSprite = GetComponent<Image>();
	}

	
	public void OnPointerEnter(PointerEventData eventData)
    {
        btnSprite.sprite = selected;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnSprite.sprite = unselected;
    }

    abstract public void OnClickLoadScene();
}
