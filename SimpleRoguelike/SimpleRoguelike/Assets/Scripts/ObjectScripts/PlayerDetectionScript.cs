using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionScript : MonoBehaviour {
    //private Transform parentTransform;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private MonsterScript monsterScript;
    private AudioSource audioSource;

    private bool chasable = false;
    
    //private enum DIRECTION
    //{
    //    LEFT = 0, RIGHT
    //}
    //private DIRECTION direction = DIRECTION.LEFT;
    
    private float monsterSpeed = 3.5f;


    void Start()
    {
        //parentTransform = transform.parent;
        rb = GetComponentInParent<Rigidbody2D>();
        monsterScript = GetComponentInParent<MonsterScript>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void FixedUpdate()
    {
        Move();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            chasable = true;
            playerTransform = other.transform;
            audioSource.Play(0);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            chasable = false;
            audioSource.Stop();
        }
    }

    void Move()
    {
        if(chasable)
        {
            int moveV = 0, moveH = 0;

            //if(parentTransform.position.x > playerTransform.position.x + 0.1)
            //{
            //    moveH = 1;
            //}
            //else if(parentTransform.position.x < playerTransform.position.x - 0.1)
            //{
            //    moveH = -1;
            //}

            //if(parentTransform.position.y > playerTransform.position.y + 0.1)
            //{
            //    moveV = 1;
            //}
            //else if(parentTransform.position.y < playerTransform.position.y - 0.1)
            //{
            //    moveV = -1;
            //}

            //parentTransform.Translate(moveH * monsterSpeed * Time.deltaTime, moveV * monsterSpeed * Time.deltaTime, 0f);

            if (rb.transform.position.x > playerTransform.position.x + 0.1)
            {
                moveH = -1;
                //if(direction == DIRECTION.RIGHT)
                //{
                //    monsterScript.ReverseSprite();
                //    direction = DIRECTION.LEFT;
                //}
            }
            else if (rb.transform.position.x < playerTransform.position.x - 0.1)
            {
                moveH = 1;
                //if (direction == DIRECTION.LEFT)
                //{
                //    monsterScript.ReverseSprite();
                //    direction = DIRECTION.RIGHT;
                //}
            }

            if (rb.transform.position.y > playerTransform.position.y + 0.1)
            {
                moveV = -1;
            }
            else if (rb.transform.position.y < playerTransform.position.y - 0.1)
            {
                moveV = 1;
            }
            
            rb.transform.Translate(moveH * monsterSpeed * Time.deltaTime, moveV * monsterSpeed * Time.deltaTime, 0f);
        }
    }
}
