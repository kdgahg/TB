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
        for (int x = 1; x < 6; x++)
        {
            for (int y = 1; y < 6; y++)
            {
                Set(x, y, 2);
            }
        }
        UpdateTileMap();  // Start에서 UpdateTileMap을 호출하여 타일맵을 초기화
    }

    void UpdateTileMap()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                UpdateTile(x, y);
            }
        }
    }

    public void SetTile(int x, int y, int tileid)
    {
        if (tilemap == null) { tilemap = GetComponent<Tilemap>(); }
        tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[tileid]);
        tilemap = null;
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

    public int[,] ReadTileMap()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        int size_x = tilemap.size.x;
        int size_y = tilemap.size.y;


        int[,] tilemapdata = new int[size_x, size_y];

        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                int indexTile = tileSet.tiles.FindIndex(x => x == tileBase);
                tilemapdata[x, y] = indexTile;
            }
        }

        return tilemapdata;
    }
}