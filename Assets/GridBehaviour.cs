using UnityEngine;
using System.Collections.Generic;

public class GridBehaviour : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 10; // ����(y)
    public int columns = 10; // ����(x)
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public List<GameObject> path = new List<GameObject>();

    public GameObject objectToMove;  // ������ ��ü
    public float moveSpeed = 1.0f;   // �ӵ� ���� ����
    public bool isMoving = false;    // ������ ���� �÷���

    private int currentPathIndex = 0;

    void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
            GenerateGrid();
        else
            Debug.LogWarning("GridPrefab�� �����ϴ�. �ν����Ϳ��� �Ҵ��� �ּ���.");
    }

    void Update()
    {
        if (findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
            currentPathIndex = 0; // ��� Ž���� ������ �ε��� �ʱ�ȭ
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
            Debug.Log("��ǥ ��ġ�� ������ �� �����ϴ�.");
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
                return; // ��ο� `null`�� ���� ��� �ε��� ���� �� �Լ� ����
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
            isMoving = false; // ��� ���� �����ϸ� �������� ����

            // ������ ���� �� startX�� startY�� endX�� endY�� ������Ʈ
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
            case 1: // ��
                if (y + 1 < rows && gridArray[x, y + 1] != null && gridArray[x, y + 1].GetComponent<GridStat>() != null && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
                    return true;
                break;
            case 2: // ������
                if (x + 1 < columns && gridArray[x + 1, y] != null && gridArray[x + 1, y].GetComponent<GridStat>() != null && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                break;
            case 3: // �Ʒ�
                if (y - 1 >= 0 && gridArray[x, y - 1] != null && gridArray[x, y - 1].GetComponent<GridStat>() != null && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                    return true;
                break;
            case 4: // ����
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
