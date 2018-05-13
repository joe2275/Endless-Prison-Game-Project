using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // 플레이어 상수
    enum STATES
    {
        IDLE = 0, WALK_LEFT, WALK_RIGHT, DEAD
    };
    const int MAX = 300;
    const int STEMINA_GENERATION = 5, STEMINA_CONSUME = 8;
    const int BATTERY_CONSUME = 1, BATTERY_GET = 100;
    const float LIGHT_MAX = 10;
    const float LIGHT_RANGE_MAX = 5;
    const float ANIMATION_TIME_TERM = 0.2f;

    private PlayerManagerScript playerManager;
    private Rigidbody2D rb;
    private SpriteRenderer render;
    private AudioSource pickupSound;
    // 플레이어 애니메이션 스프라이트
    public Sprite[] idle;
    public Sprite[] walkRight;
    public Sprite[] walkLeft;
    public Sprite dead;
    private int spriteCount = 0;

    // 플레이어 스피드
    public int playerSpeed;
    public int playerRunningSpeed;
    // 플레이어가 달리는 경우(LeftShift)
    private bool isRunning = false;
    // 플레이어 스테미나가 다 닳면 3초동안 기다리는 플래그
    private bool isRecharging = false;
    // 배터리가 거의 없을때 닳는것을 잠시 중지하기 위한 플래그
    private bool waitForBattery = false;
    // 플레이어가 열쇠를 가지고 있는지 표시하는 플래그
    private bool hasKey = false;

    private bool showMenu = false;

    // 플레이어 배터리 잔량과 스테미나
    private int battery = MAX;
    private int stemina = MAX;

    // 애니메이션 전환 주기를 설정하는 플래그
    private bool animChangable = true;
    
    // 플레이어의 상태
    private STATES state = STATES.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        pickupSound = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerManager = PlayerManagerScript.GetInstance();
        StartCoroutine(AdjustGauge1());
        StartCoroutine(AdjustGauge2());
    }

    void FixedUpdate()
    {
        if (state != STATES.DEAD)
        {
            CheckRunning();
            Move();
        }
        else
        {
            /*
             * 게임 오버 관련 메소드 작동
             */
            render.sprite = dead;
            rb.bodyType = RigidbodyType2D.Static;
            if (!showMenu)
            {
                StartCoroutine(playerManager.PlayerDeadNotify());
                showMenu = true;
            }
        }
    }

    void Update()
    {
        PlayAnimation();
    }

    private void Move()
    {
        float moveV = Input.GetAxisRaw("Vertical");
        float moveH = Input.GetAxisRaw("Horizontal");

        // 플레이어가 달리지 않는 경우와 달리는 경우
        if (!isRunning)
        {
            rb.transform.Translate(moveH * playerSpeed * Time.deltaTime, moveV * playerSpeed * Time.deltaTime, 0f);
        }
        else if(state != STATES.IDLE)
        {
            rb.transform.Translate(moveH * playerRunningSpeed * Time.deltaTime, moveV * playerRunningSpeed * Time.deltaTime, 0f);
        }
        // 플레이어 상태 설정
        if (moveH < -0.1f)
        {

            state = STATES.WALK_LEFT;
        }
        else if (moveH > 0.1f || moveV != 0f)
        {
            state = STATES.WALK_RIGHT;
        }
        else
        {
            state = STATES.IDLE;
        }
    }

    // Stemina, Battery의 게이지를 조정하는 코루틴 메서드
    private IEnumerator AdjustGauge1()
    {
        while(true)
        {
            if(!isRunning && stemina < MAX)
            {
                stemina += STEMINA_GENERATION;
            }
            if (!waitForBattery)
            {
                battery -= BATTERY_CONSUME;
                if (battery <= 0)
                {
                    state = STATES.DEAD;
                    battery = 0;
                }

                playerManager.GetBatterySlider().value = ((float)battery) / MAX;

                if (battery == 3)
                {
                    StartCoroutine(WaitForBatteryDead());
                }
            }
            playerManager.GetPlayerLight().intensity = ((float)battery) / MAX * LIGHT_MAX;
            playerManager.GetPlayerLight().range = LIGHT_RANGE_MAX + ((float)battery) / MAX * LIGHT_RANGE_MAX;
            playerManager.GetSteminaSlider().value = ((float)stemina) / MAX;

            yield return new WaitForSeconds(0.3f);
        }
    }
    private IEnumerator AdjustGauge2()
    {
        while(true)
        {
            if(isRunning && state != STATES.IDLE)
            {
                stemina -= STEMINA_CONSUME;
                if(stemina <= 0)
                {
                    stemina = 0;
                    isRecharging = true;
                    StartCoroutine(WaitForRechargeStemina());
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    private IEnumerator WaitForRechargeStemina()
    {
        yield return new WaitForSeconds(3f);
        isRecharging = false;
    }
    private IEnumerator WaitForBatteryDead()
    {
        waitForBattery = true;
        yield return new WaitForSeconds(10f);
        waitForBattery = false;
    }

    private void CheckRunning()
    {
        isRunning = (!isRecharging && Input.GetKey(KeyCode.LeftShift) ? true : false);
    }

    // 플레이어의 상태에 따른 애니메이션 변경
    private void PlayAnimation()
    {
        if (animChangable)
        {
            if (state == STATES.IDLE)
            {
                if (spriteCount >= idle.Length)
                    spriteCount = 0;

                render.sprite = idle[spriteCount];
            }
            else if (state == STATES.WALK_LEFT)
            {
                if (spriteCount >= walkLeft.Length)
                    spriteCount = 0;

                render.sprite = walkLeft[spriteCount];
            }
            else if (state == STATES.WALK_RIGHT)
            {
                if (spriteCount >= walkRight.Length)
                    spriteCount = 0;

                render.sprite = walkRight[spriteCount];
            }

            spriteCount++;
            StartCoroutine(WaitAnimation());
        }
    }

    // 플레이어 애니메이션을 재생하는 코루틴 메소드
    private IEnumerator WaitAnimation()
    {
        animChangable = false;
        if (!isRunning)
        {
            yield return new WaitForSeconds(ANIMATION_TIME_TERM);
        }
        else
        {
            yield return new WaitForSeconds(ANIMATION_TIME_TERM / 2);
        }
        animChangable = true;
    }

    // 플레이어가 배터리와 접촉할때 호출되는 메소드
    public void GetBattery()
    {
        battery = MAX;
        waitForBattery = false;
    }

    // 플레이어가 몬스터와 접촉할때 호출, 플레이어가 죽음
    public void Dead()
    {
        state = STATES.DEAD;
    }

    // 플레이어가 열쇠를 주웠을때 호출하는 메소드
    public void SetKey()
    {
        hasKey = true;
        pickupSound.Play();
    }

    // 플레이어가 열쇠를 가지고 있는지 체크하는 메소드
    public bool HasKey()
    {
        return hasKey;
    }
}
