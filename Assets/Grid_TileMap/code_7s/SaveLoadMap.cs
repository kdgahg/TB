using UnityEngine;

public class SaveLoadMap : MonoBehaviour
{
    [SerializeField] MapData mapData;

    [SerializeField] GridMap gridMap;
    [SerializeField] GridManager gridManager;
    public void Save()
    {
        int[,] map = gridManager.ReadTileMap();
        mapData.Save(map);
    }

    public void Load()
    {
        gridManager.Clear();

        int width = mapData.width;
        int height = mapData.height;

        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridManager.SetTile(x, y, mapData.map[i]);
                i += 1;
            }
        }
    }
    internal void Load(GridMap grid)
    {
        mapData.Load(grid);
    }
}