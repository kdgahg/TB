using UnityEngine;
using System.Collections.Generic;

public class GridBehaviour : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 10; // 세로(y)
    public int columns = 10; // 가로(x)
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public List<GameObject> path = new List<GameObject>();

    public GameObject objectToMove;  // 움직일 객체
    public float moveSpeed = 1.0f;   // 속도 조절 변수
    public bool isMoving = false;    // 움직임 제어 플래그

    private int currentPathIndex = 0;

    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
            GenerateGrid();
        else
            Debug.LogWarning("GridPrefab이 없습니다. 인스펙터에서 할당해 주세요.");
    }

    void Update()
    {
        if (findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
            currentPathIndex = 0; // 경로 탐색이 끝나면 인덱스 초기화
        }

        if (objectToMove != null && isMoving)
        {
            MoveAlongPath();
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y + scale * j, leftBottomLocation.z), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = i;
                obj.GetComponent<GridStat>().y = j;
                gridArray[i, j] = obj;
            }
        }
    }

    void SetDistance()
    {
        InitialSetup();
        for (int step = 1; step < columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                if (obj != null && obj.GetComponent<GridStat>() != null && obj.GetComponent<GridStat>().visited == step - 1)
                {
                    TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
                }
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();

        if (gridArray[endX, endY] != null && gridArray[endX, endY].GetComponent<GridStat>() != null && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            Debug.Log("목표 위치에 도달할 수 없습니다.");
            return;
        }

        for (int i = step; i > -1; i--)
        {
            if (TestDirection(x, y, i, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, i, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, i, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, i, 4))
                tempList.Add(gridArray[x - 1, y]);

            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            if (tempObj != null)
            {
                path.Add(tempObj);
                x = tempObj.GetComponent<GridStat>().x;
                y = tempObj.GetComponent<GridStat>().y;
            }
            tempList.Clear();
        }
        path.Reverse();
    }

    void MoveAlongPath()
    {
        if (objectToMove == null || path.Count == 0)
            return;

        if (currentPathIndex < path.Count)
        {
            GameObject targetNode = path[currentPathIndex];
            if (targetNode == null)
            {
                currentPathIndex++;
                return; // 경로에 `null`이 있을 경우 인덱스 증가 및 함수 종료
            }

            Vector3 targetPosition = targetNode.transform.position;
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(objectToMove.transform.position, targetPosition) < 0.01f)
            {
                currentPathIndex++;
            }
        }
        else
        {
            isMoving = false; // 경로 끝에 도달하면 움직임을 멈춤

            // 움직임 종료 후 startX와 startY를 endX와 endY로 업데이트
            startX = endX;
            startY = endY;
        }
    }

    void InitialSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj != null && obj.GetComponent<GridStat>() != null)
                obj.GetComponent<GridStat>().visited = -1;
        }
        if (gridArray[startX, startY] != null && gridArray[startX, startY].GetComponent<GridStat>() != null)
            gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        switch (direction)
        {
            case 1: // 위
                if (y + 1 < rows && gridArray[x, y + 1] != null && gridArray[x, y + 1].GetComponent<GridStat>() != null && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
                    return true;
                break;
            case 2: // 오른쪽
                if (x + 1 < columns && gridArray[x + 1, y] != null && gridArray[x + 1, y].GetComponent<GridStat>() != null && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                break;
            case 3: // 아래
                if (y - 1 >= 0 && gridArray[x, y - 1] != null && gridArray[x, y - 1].GetComponent<GridStat>() != null && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                    return true;
                break;
            case 4: // 왼쪽
                if (x - 1 >= 0 && gridArray[x - 1, y] != null && gridArray[x - 1, y].GetComponent<GridStat>() != null && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                break;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }

    void SetVisited(int x, int y, int step)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows && gridArray[x, y] != null)
            gridArray[x, y].GetComponent<GridStat>().visited = step;
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject obj in list)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(targetLocation.position, obj.transform.position);
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    closest = obj;
                }
            }
        }
        return closest;
    }
}
