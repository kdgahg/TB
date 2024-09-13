using UnityEngine;

public class GridMap : MonoBehaviour
{
    [HideInInspector]
    public int height;  // 접근 제한자를 public으로 설정
    [HideInInspector]
    public int width;

    int[,] grid;

    public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new int[width, height];
    }

    public void Set(int x, int y, int to)
    {
        if (CheckPosition(x, y) == false)
        {
            Debug.LogWarning("Trying to Set an cell the Grid boundaries"
               + x.ToString() + ":" + y.ToString());
            return;
        }

        grid[x, y] = to;
    }

    public int Get(int x, int y)
    {
        if (CheckPosition(x, y) == false)
        {
            Debug.LogWarning("Trying to Get an cell the Grid boundaries"
                + x.ToString() + ":" + y.ToString());
            return -1;
        }
        return grid[x, y];
    }

    public bool CheckPosition(int x, int y)
    {
        if (x < 0 || x >= width)
        {
            return false;
        }
        if (y < 0 || y >= height)
        {
            return false;
        }

        return true;
    }

    internal bool CheckWalkable(int xPos, int yPos)
    {
        return grid[xPos, yPos] == 0;
    }
}