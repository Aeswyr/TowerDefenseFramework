using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    // the movement classification of this type of tile
    public enum WalkType {
        Walkable, // "enemies" can move on/through this tile
        Obstacle, // "enemies" cannot move on/through this tile

        // Specify additional types below
        Water
    }

    public TileBase[] Tiles;

    [SerializeField]
    private TileData.WalkType m_walkType;

    public TileData.WalkType GetWalkType() {
        return m_walkType;
    }

}
