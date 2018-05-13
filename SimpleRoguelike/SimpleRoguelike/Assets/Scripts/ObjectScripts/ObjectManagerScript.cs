using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerScript : MonoBehaviour {
    private static ObjectManagerScript instance = null;

    public GameObject wallPrefab;
    public GameObject batteryPrefab;
    public GameObject monsterPrefab;
    public GameObject exitPrefab;
    public GameObject keyPrefab;
    public GameObject lockerPrefab;

    // 맵의 가로 세로 크기
    public int width;
    public int height;
    public int numOfBifurcation;
    public int numOfBarrier;
    // [행, 열]
    private int[,] map;

    // 도착 지점
    private Point end;

    private int numOfMonster = 1;

    const int WALL = 0, BIFURCATION = 1, BARRIER = 2, WAY = 3;

    public static ObjectManagerScript GetInstance()
    {
        if (instance == null) instance = FindObjectOfType<ObjectManagerScript>();
        return instance;
    }

	// Use this for initialization
	void Start () {
        map = new int[height, width];
        
        InitializeWallArray();
        InstantiateWall();
        InstantiateMonster();

        Debug.Log(end.x + ", " + end.y);
    }

    // 맵 상에서 벽을 생성하는 메소드
    private void InitializeWallArray()
    {
        // 도착 지점을 설정
        InitializeEndPoint();
        // 분기점의 위치를 보관하는 큐
        Queue<Point> bifurcationQueue = new Queue<Point>();
        // 막다른 길의 위치를 보관하는 큐
        Queue<Point> barrierQueue = new Queue<Point>();
        // 분기점의 개수를 세는 변수
        int countOfBifurcation = 0;
        // 막다른 길의 개수를 세는 변수
        int countOfBarrier = 0;
        // 임시 좌표를 보관하는 변수
        int x, y;
        // 시작 지점 BARRIER 설정
        map[0, 0] = BARRIER;
        map[end.y, end.x] = BARRIER;

        // 분기점의 좌표를 무작위로 생성
        while(countOfBifurcation < numOfBifurcation)
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);

            if(map[y, x] == WALL)
            {
                map[y, x] = BIFURCATION;
                bifurcationQueue.Enqueue(new Point(x, y));
                countOfBifurcation++;
            }
        }

        // 도착 지점 분기점 설정

        barrierQueue.Enqueue(new Point(0, 0));

        // 막다른 길의 좌표를 무작위로 생성
        while (countOfBarrier < numOfBarrier)
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);

            if (map[y, x] == WALL)
            {
                map[y, x] = BARRIER;
                barrierQueue.Enqueue(new Point(x, y));
                countOfBarrier++;
            }
        }

        barrierQueue.Enqueue(end);

        Point prevPoint = bifurcationQueue.Dequeue();
        bifurcationQueue.Enqueue(prevPoint);
        Point point;
        // 분기점들 끼리 연결 (탈출 분기점도 포함 + 1)
        while(countOfBifurcation + 1 > 0)
        {
            // x축, y축으로 진행해야 되는 방향
            int xDirection, yDirection;

            // 끝점 연결을 위해서 다시 큐에 넣는다.
            point = bifurcationQueue.Dequeue();
            bifurcationQueue.Enqueue(point);
            countOfBifurcation--;

            // 방향 설정
            xDirection = point.x - prevPoint.x;
            xDirection = xDirection != 0 ? xDirection / Mathf.Abs(xDirection) : xDirection;
            yDirection = point.y - prevPoint.y;
            yDirection = yDirection != 0 ? yDirection / Mathf.Abs(yDirection) : yDirection;

            while (prevPoint.x != point.x || prevPoint.y != point.y)
            {
                int direction;
                // x 좌표 값이 같으면 y축으로만 진행
                if(prevPoint.x == point.x)
                {
                    direction = 1;
                } // y 좌표 값이 같으면 x 축으로만 진행
                else if(prevPoint.y == point.y)
                {
                    direction = 0;
                }
                else
                {
                    direction = (int)Random.Range(0, 2);
                }


                switch(direction)
                {
                    // x 축으로 진행
                    case 0:
                        prevPoint.x += xDirection;
                        break;
                    // y 축으로 진행
                    case 1:
                        prevPoint.y += yDirection;
                        break;
                }
                // 맵상에서 길로 설정
                map[prevPoint.y, prevPoint.x] = WAY;
            }

            prevPoint = point;
        }
        
        // 분기점과 막힌 지점들 끼리 연결
        while (barrierQueue.Count > 0)
        {
            // x축, y축으로 진행해야 되는 방향
            int xDirection, yDirection;

            // 끝점 연결을 위해서 다시 큐에 넣는다.
            prevPoint = bifurcationQueue.Dequeue();
            bifurcationQueue.Enqueue(prevPoint);
            point = barrierQueue.Dequeue();
            countOfBarrier--;

            // 방향 설정
            xDirection = point.x - prevPoint.x;
            xDirection = xDirection != 0 ? xDirection / Mathf.Abs(xDirection) : xDirection;
            yDirection = point.y - prevPoint.y;
            yDirection = yDirection != 0 ? yDirection / Mathf.Abs(yDirection) : yDirection;

            while (prevPoint.x != point.x || prevPoint.y != point.y)
            {
                int direction;
                // x 좌표 값이 같으면 y축으로만 진행
                if (prevPoint.x == point.x)
                {
                    direction = 1;
                } // y 좌표 값이 같으면 x 축으로만 진행
                else if (prevPoint.y == point.y)
                {
                    direction = 0;
                }
                else
                {
                    direction = (int)Random.Range(0, 2);
                }


                switch (direction)
                {
                    // x 축으로 진행
                    case 0:
                        prevPoint.x += xDirection;
                        break;
                    // y 축으로 진행
                    case 1:
                        prevPoint.y += yDirection;
                        break;
                }
                // 맵상에서 길로 설정
                map[prevPoint.y, prevPoint.x] = WAY;
            }
        }

    }

    private void InstantiateWall()
    {
        int x = 0, y = 0;

        for(x = 0; x<width; x++)
        {
            for(y = 0; y<height; y++)
            {
                if (map[y, x] == WALL)
                {
                    GameObject wall = Instantiate(wallPrefab, transform);
                    wall.transform.position = new Vector3((float)x * 2, (float)y * 2, -1f);
                }
                else
                {
                    int possibility = (int)Random.Range(0.0f, 200.0f);
                    if (possibility == 0 || possibility == 1)
                    {
                        GameObject battery = Instantiate(batteryPrefab, transform);
                        battery.transform.position = new Vector3((float)x * 2, (float)y * 2, -0.5f);
                    }
                }
                
            }
        }

        // 플레이어가 3층 이상 진입할 경우 출구에 Locker 생성
        if (PlayerManagerScript.GetInstance().GetFloorTextScript().GetFloor() >= 3)
        {
            GameObject locker = Instantiate(lockerPrefab, transform);
            locker.transform.position = new Vector3((float)end.x * 2, (float)end.y * 2, -1f);
        }

        GameObject exit = Instantiate(exitPrefab, transform);
        exit.transform.position = new Vector3((float)end.x * 2, (float)end.y * 2, -0.5f);
    }

    private void InitializeEndPoint()
    {
        int way = (int)Random.Range(0.0f, 2.0f);
        // way 가 0이면 상단 좌표 중 하나, 1이면 우측 좌표 중 하나가 반환된다.
        end = (way == 0 ? new Point((int)Random.Range(0.0f, width), height - 1) :
            new Point(width - 1, (int)Random.Range(0.0f, height)));
    }

    private void InstantiateMonster()
    {
        int x = 0, y = 0;
        numOfMonster = PlayerManagerScript.GetInstance().GetFloorTextScript().GetFloor();
        for (int i = 0; i < numOfMonster; i++)
        {
            do
            {
                x = (int)Random.Range(0.0f, width);
                y = (int)Random.Range(0.0f, height);
            } while (map[y, x] == WALL || (x == end.x && y == end.y) || (x < 5 && y < 5));
            GameObject monsterObject = Instantiate(monsterPrefab, transform);
            monsterObject.transform.position = new Vector3((float)x * 2, (float)y * 2, -1.0f);
        }
    }

    public void InstantiateKey()
    {
        int x, y;
        do
        {
            x = (int)Random.Range(0.0f, width);
            y = (int)Random.Range(0.0f, height);
        } while (map[y, x] == WALL || x == end.x && y == end.y);

        GameObject keyObject = Instantiate(keyPrefab, transform);
        keyObject.transform.position = new Vector3((float)x * 2, (float)y * 2, -1.0f);
    }
}

// 큐에 저장하는 클래스 타입
public class Point
{
    public int x;
    public int y;

    public Point(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}