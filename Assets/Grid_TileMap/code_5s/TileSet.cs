using System.Collections.Generic;  // List를 사용하려면 필요
using UnityEngine;
using UnityEngine.Tilemaps;  // TileBase를 사용하려면 필요

[CreateAssetMenu(menuName = "Tile Set")]
public class TileSet : ScriptableObject
{
    public List<TileBase> tiles;
}
