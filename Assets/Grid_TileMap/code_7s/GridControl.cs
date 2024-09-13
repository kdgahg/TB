using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;  // Ÿ�ϸ��� ����
    [SerializeField] GridManager gridManager;  // GridManager ����
    Pathfinding pathFinding;  // Pathfinding ������Ʈ

    int currentX = 0;  // ���� ��ġ X
    int currentY = 0;  // ���� ��ġ Y
    int targetPosX = 0;  // ��ǥ ��ġ X
    int targetPosY = 0;  // ��ǥ ��ġ Y

    [SerializeField] TileBase highlightTile;  // ��θ� ���̶���Ʈ�� Ÿ��

    private void Start()
    {
        // Pathfinding ������Ʈ �ʱ�ȭ
        pathFinding = gridManager.GetComponent<Pathfinding>();
        if (pathFinding == null)
        {
            Debug.LogError("Pathfinding ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void Update()
    {
        MouseInput();  // ���콺 �Է� ó��
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))  // ���콺 ���� Ŭ�� ����
        {
            // Ŭ���� �� ���� Ÿ���� ��� ���� (����ȭ �ʿ� �� ���� ����)
            targetTilemap.ClearAllTiles();

            // ���콺 Ŭ���� ��ġ�� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);

            // Ŭ���� ��ǥ�� Ÿ�ϸ� ���� ���� �ִ��� Ȯ��
            if (!gridManager.GetComponent<GridMap>().CheckPosition(clickPosition.x, clickPosition.y))
            {
                Debug.LogError("Ŭ���� ��ǥ�� Ÿ�ϸ� ������ ������ϴ�.");
                return;
            }

            targetPosX = clickPosition.x;
            targetPosY = clickPosition.y;

            // ��� Ž��
            List<PathNode> path = pathFinding.FindPath(currentX, currentY, targetPosX, targetPosY);

            // ��ΰ� �����ϸ� ���̶���Ʈ Ÿ�Ϸ� ǥ��
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    targetTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0), highlightTile);
                }

                // ���� ��ġ ����
                currentX = targetPosX;
                currentY = targetPosY;
            }
            else
            {
                Debug.LogWarning("��θ� ã�� ���߽��ϴ�.");
            }
        }
    }
}
