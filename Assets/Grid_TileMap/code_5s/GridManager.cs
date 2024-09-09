using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMap))]
[RequireComponent(typeof(Tilemap))]
public class GridManager : MonoBehaviour
{
    Tilemap tilemap;  // 'TileMap'이 아닌 'Tilemap'으로 수정
    GridMap grid;  // CustomGrid 대신 GridMap 사용

    [SerializeField] TileSet tileSet;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();  // 'TileMap'이 아닌 'Tilemap'으로 수정
        grid = GetComponent<GridMap>();  // CustomGrid 대신 GridMap 사용
        grid.Init(10, 8);  // 필드 초기화
        Set(1, 1, 2);
        Set(1, 2, 2);
        Set(2, 1, 2);
        UpdateTileMap();  // Start에서 UpdateTileMap을 호출하여 타일맵을 초기화
    }

    void UpdateTileMap()
    {
        for (int x = 0; x < grid.length; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                UpdateTile(x, y);
            }
        }
    }

    private void UpdateTile(int x, int y)
    {
        int tileId = grid.Get(x, y);
        if (tileId == -1)
        {
            return;
        }

        tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[tileId]);
    }

    public void Set(int x, int y, int to)
    {
        grid.Set(x, y, to);
        UpdateTile(x, y);
    }
}
