using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public Sprite[] monsterSprite;
    private SpriteRenderer render;

    private int spriteCount = 0;

    const float ANIMATION_TIME_TERM = 0.2f;

    private bool animChangable = true;

    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        PlayAnimation();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // 플레이어가 몬스터랑 접촉했을 경우
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManagerScript.GetInstance().GetPlayerScript().Dead();
        }
    }

    // 플레이어 애니메이션을 재생하는 메소드
    private void PlayAnimation()
    {
        if (animChangable)
        {
            render.sprite = monsterSprite[spriteCount];
            spriteCount = (spriteCount + 1) % monsterSprite.Length;
            StartCoroutine(WaitAnimation());
        }
    }

    // 몬스터 애니메이션의 Time Term을 대기하는 코루틴 메서드
    IEnumerator WaitAnimation()
    {
        animChangable = false;
        yield return new WaitForSeconds(ANIMATION_TIME_TERM);
        animChangable = true;
    }

    // 몬스터의 방향을 반대로 향하게 하는 메서드
    public void ReverseSprite()
    {
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1f;
        transform.localScale = playerScale;
    }
}