using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;  // 타일맵을 지정
    [SerializeField] GridManager gridManager;  // GridManager 연결
    Pathfinding pathFinding;  // Pathfinding 컴포넌트

    int currentX = 0;  // 현재 위치 X
    int currentY = 0;  // 현재 위치 Y
    int targetPosX = 0;  // 목표 위치 X
    int targetPosY = 0;  // 목표 위치 Y

    [SerializeField] TileBase highlightTile;  // 경로를 하이라이트할 타일

    private void Start()
    {
        // Pathfinding 컴포넌트 초기화
        pathFinding = gridManager.GetComponent<Pathfinding>();
        if (pathFinding == null)
        {
            Debug.LogError("Pathfinding 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        MouseInput();  // 마우스 입력 처리
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스 왼쪽 클릭 감지
        {
            // 클릭한 후 기존 타일을 모두 지움 (최적화 필요 시 변경 가능)
            targetTilemap.ClearAllTiles();

            // 마우스 클릭한 위치의 화면 좌표를 월드 좌표로 변환
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);

            // 클릭한 좌표가 타일맵 범위 내에 있는지 확인
            if (!gridManager.GetComponent<GridMap>().CheckPosition(clickPosition.x, clickPosition.y))
            {
                Debug.LogError("클릭한 좌표가 타일맵 범위를 벗어났습니다.");
                return;
            }

            targetPosX = clickPosition.x;
            targetPosY = clickPosition.y;

            // 경로 탐색
            List<PathNode> path = pathFinding.FindPath(currentX, currentY, targetPosX, targetPosY);

            // 경로가 존재하면 하이라이트 타일로 표시
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    targetTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0), highlightTile);
                }

                // 현재 위치 갱신
                currentX = targetPosX;
                currentY = targetPosY;
            }
            else
            {
                Debug.LogWarning("경로를 찾지 못했습니다.");
            }
        }
    }
}
