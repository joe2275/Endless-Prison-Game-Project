using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureObjectManager : MonoBehaviour
{

    public GameObject wallPrefab;
    public GameObject batteryPrefab;
    public GameObject monsterPrefab;
    public GameObject exitPrefab;
    public GameObject keyPrefab;
    public GameObject lockerPrefab;
    public GameObject floorPrefab;

    const int SIZE_ONE_BLOCK = 2;
    const float INIT_ABS_POSITION = 10f;

    public Transform playerTransform;

    private Transform boardHolder;

    private Dictionary<Vector2, Vector2> gridPosition;

    private void Start()
    {
        boardHolder = new GameObject("Board").transform;
        gridPosition = new Dictionary<Vector2, Vector2>();
        initializeFloor();
    }

    private void Update()
    {
        checkAndGerateObject();
    }

    // 처음 플레이어를 기준으로 10X10 Floor 타일을 생성
    private void initializeFloor()
    {
        GameObject tempObject;
        Vector2 tempPosition;

        for (float i = (playerTransform.position.x - INIT_ABS_POSITION); i <= playerTransform.position.x + INIT_ABS_POSITION; i += SIZE_ONE_BLOCK)
        {
            for (float j = (playerTransform.position.y - INIT_ABS_POSITION); j <= playerTransform.position.y + INIT_ABS_POSITION; j += SIZE_ONE_BLOCK)
            {
                tempPosition = new Vector2(i, j);
                gridPosition.Add(tempPosition, tempPosition);
                tempObject = Instantiate(floorPrefab, new Vector3(i, j, 0.0f), Quaternion.identity);
                tempObject.transform.SetParent(boardHolder);
            }
        }
    }

    // 현재 플레이어의 위치 기준으로 짝수 좌표 반환
    private Vector2 processPlayerPosition()
    {
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;

        if (Mathf.Floor(playerX) % 2 != 0f)
        {
            playerX = playerX - 1f;
        }
        if (Mathf.Floor(playerY) % 2 != 0f)
        {
            playerY = playerY - 1f;
        }

        return new Vector2(playerX, playerY);
    }

    // 플레이어 위치를 기준으로 생성한 오브젝트인지 검사하고 생성되지 않았으면 오브젝트 랜덤 생성
    private void checkAndGerateObject()
    {
        Vector2 playerPosition = processPlayerPosition();
   
        for (float i = playerPosition.y - INIT_ABS_POSITION; i <= playerPosition.y + INIT_ABS_POSITION; i += SIZE_ONE_BLOCK)
        {
            // 플레이어 오른쪽 방향에 오브젝트가 생성되었는지 체크 후 생성
            for (float j = playerPosition.x + INIT_ABS_POSITION/* - SIZE_ONE_BLOCK*/; j <= playerPosition.x + INIT_ABS_POSITION; j += SIZE_ONE_BLOCK)
            {
                generateObject(new Vector2(Mathf.Floor(j), Mathf.Floor(i)));
            }
            // 플레이어 왼쪽 방향에 오브젝트가 생성되었는지 체크 후 생성
            for (float j = playerPosition.x - INIT_ABS_POSITION/* + SIZE_ONE_BLOCK*/; j >= playerPosition.x - INIT_ABS_POSITION; j -= SIZE_ONE_BLOCK)
            {
                generateObject(new Vector2(Mathf.Floor(j), Mathf.Floor(i)));
            }
        }
        for (float i = playerPosition.x - INIT_ABS_POSITION; i <= playerPosition.x + INIT_ABS_POSITION; i += SIZE_ONE_BLOCK)
        {
            // 플레이어 위쪽 방향에 오브젝트가 생성되었는지 체크 후 생성
            for (float j = playerPosition.y + INIT_ABS_POSITION/* - SIZE_ONE_BLOCK*/; j <= playerPosition.y + INIT_ABS_POSITION; j += SIZE_ONE_BLOCK)
            {
                generateObject(new Vector2(Mathf.Floor(i), Mathf.Floor(j)));
            }
            // 플레이어 아래쪽 방향에 오브젝트가 생성되었는지 체크 후 생성
            for (float j = playerPosition.y - INIT_ABS_POSITION/* + SIZE_ONE_BLOCK*/; j >= playerPosition.y - INIT_ABS_POSITION; j -= SIZE_ONE_BLOCK)
            {
                generateObject(new Vector2(Mathf.Floor(i), Mathf.Floor(j)));
            }
        }
    }

    private void generateObject(Vector2 position)
    {
        GameObject tempObject;

        if (!gridPosition.ContainsKey(position))
        {
            Debug.Log(position);
            gridPosition.Add(position, position);
            tempObject = Instantiate(floorPrefab, position, Quaternion.identity);
            tempObject.transform.SetParent(boardHolder);

            /* 아이템, 몬스터 랜덤 생성 */

        }
    }
}
