using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagerScript : MonoBehaviour {
    private static PlayerManagerScript playerManager = null;

    private PlayerScript playerScript;
    private Slider batterySlider;
    private Slider steminaSlider;
    private Light playerLight;
    private FloorTextScript floorTextScript;

    private RetryButtonScript retryButtonScript;
    private DeadImageScript deadImageScript;

    public static PlayerManagerScript GetInstance()
    {
        if (playerManager == null) playerManager = FindObjectOfType<PlayerManagerScript>();
        return playerManager;
    }

	void Awake () {
        playerScript = FindObjectOfType<PlayerScript>();
        batterySlider = FindObjectOfType<BatteryScript>().GetComponent<Slider>();
        steminaSlider = FindObjectOfType<SteminaScript>().GetComponent<Slider>();
        playerLight = FindObjectOfType<LightScript>().GetComponent<Light>();
        retryButtonScript = FindObjectOfType<RetryButtonScript>();
        deadImageScript = FindObjectOfType<DeadImageScript>();
        retryButtonScript.gameObject.SetActive(false);
        deadImageScript.gameObject.SetActive(false);
        floorTextScript = FindObjectOfType<FloorTextScript>();

    }

    void Update()
    {
        CheckESC();
    }

    // ESC 키를 누르면 종료
    private void CheckESC()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public PlayerScript GetPlayerScript()
    {
        return playerScript;
    }
    public Slider GetBatterySlider()
    {
        return batterySlider;
    }
    public Slider GetSteminaSlider()
    {
        return steminaSlider;
    }
    public Light GetPlayerLight()
    {
        return playerLight;
    }
    public FloorTextScript GetFloorTextScript()
    {
        return floorTextScript;
    }

    // 플레이어가 죽었음을 알림받는 메소드
    public IEnumerator PlayerDeadNotify()
    {
        yield return new WaitForSeconds(3f);
        retryButtonScript.gameObject.SetActive(true);
        deadImageScript.gameObject.SetActive(true);
    }
}
