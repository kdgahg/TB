using System.Collections.Generic;  // List�� ����Ϸ��� �ʿ�
using UnityEngine;
using UnityEngine.Tilemaps;  // TileBase�� ����Ϸ��� �ʿ�

[CreateAssetMenu(menuName = "Tile Set")]
public class TileSet : ScriptableObject
{
    public List<TileBase> tiles;
}
