using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMap))]
[RequireComponent(typeof(Tilemap))]
public class GridManager : MonoBehaviour
{
    Tilemap tilemap;  // 'TileMap'�� �ƴ� 'Tilemap'���� ����
    GridMap grid;  // CustomGrid ��� GridMap ���

    [SerializeField] TileSet tileSet;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();  // 'TileMap'�� �ƴ� 'Tilemap'���� ����
        grid = GetComponent<GridMap>();  // CustomGrid ��� GridMap ���
        grid.Init(10, 8);  // �ʵ� �ʱ�ȭ
        Set(1, 1, 2);
        Set(1, 2, 2);
        Set(2, 1, 2);
        UpdateTileMap();  // Start���� UpdateTileMap�� ȣ���Ͽ� Ÿ�ϸ��� �ʱ�ȭ
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
