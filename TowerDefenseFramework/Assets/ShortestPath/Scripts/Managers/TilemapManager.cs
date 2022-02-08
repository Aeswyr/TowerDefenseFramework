
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    public static TilemapManager instance;

    #region TileData

    [SerializeField]
    private List<TileData> m_tileDataList;

    [SerializeField]
    private Dictionary<TileBase, TileData> m_tileDataDict;

    #endregion

    //  TODO: dynamically load this for each level
    [SerializeField]
    private Tilemap m_map;

    [SerializeField]
    private Destination m_destination;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (this != instance) {
            Debug.Log("Warning! You have multiple TilemapManagers simultaneously. This may result in unexpected behavior.");
        }

        m_tileDataDict = new Dictionary<TileBase, TileData>();

        foreach(TileData tileData in m_tileDataList) {
            foreach (var tile in tileData.Tiles) {
                m_tileDataDict.Add(tile, tileData);
            }
        }

        if (m_map == null) {
            Debug.Log("No Tilemap assigned. Shortest paths cannot be calculated.");
        }
    }

    public List<Vector2> CalculatePath(List<TileData.WalkType> canWalkOn, Vector2 startPos) {

        // canWalkOn : the types of terrain the given oncomer can walk through
        int[,] mapArray = ConvertMapToArray(canWalkOn);

        // get starting position in array terms

        Vector3Int startGridPos = m_map.WorldToCell(startPos);
        Vector2Int startArrPos = new Vector2Int(startGridPos.x - (m_map.cellBounds.min.x), (m_map.cellBounds.max.y - 1) - startGridPos.y);

        Vector3Int endGridPos = m_map.WorldToCell(m_destination.transform.position);
        Vector2Int endArrPos = new Vector2Int(endGridPos.x - (m_map.cellBounds.min.x), (m_map.cellBounds.max.y - 1) - endGridPos.y);

        List<Vector2> shortestWaypoints = null;
        List<Vector2> currWaypoints = new List<Vector2>();

        
        CalculatePathHelper(
            mapArray,
            startArrPos,
            new Vector2Int(endArrPos.x, endArrPos.y),
            ref currWaypoints,
            ref shortestWaypoints
            );


        // convert wayPoints from arr to Tilemap

        List<Vector2> adjustedWaypoints = ConvertArrayPointsToMap(shortestWaypoints);

        return adjustedWaypoints;
    }

    // returns true if end path was reached (or could be on shorter path), somewhere down the recursion, false otherwise
    private bool CalculatePathHelper(int[,] mapArray, Vector2Int startPos, Vector2Int endPos, ref List<Vector2> currWaypoints, ref List<Vector2> shortestWaypoints) {
        if (startPos.x == endPos.x && startPos.y == endPos.y) {
            // base case: found a path to the end
            if (shortestWaypoints == null) {
                shortestWaypoints = new List<Vector2>();
                currWaypoints.Add(startPos);
                foreach (Vector2 waypoint in currWaypoints) {
                    shortestWaypoints.Add(waypoint);
                }
            }
            else if (currWaypoints.Count < shortestWaypoints.Count - 1) {
                shortestWaypoints.Clear();
                currWaypoints.Add(startPos);
                foreach (Vector2 waypoint in currWaypoints) {
                    shortestWaypoints.Add(waypoint);
                }
            }
            // backtrack
            currWaypoints.Remove(startPos);
            return true;
        }
        else {
            // mark cell as visited
            mapArray[startPos.y, startPos.x] = 2;
            currWaypoints.Add(startPos);

            if (shortestWaypoints != null && currWaypoints.Count >= shortestWaypoints.Count) {
                currWaypoints.Remove(startPos);
                mapArray[startPos.y, startPos.x] = 0;
                return true;
            }

            bool foundPath = false;

            // Recurse on the bottom cell
            if (CanMove(startPos.y + 1, startPos.x, mapArray)) {
                if (CalculatePathHelper(
                    mapArray,
                    new Vector2Int(startPos.x, startPos.y + 1),
                    endPos,
                    ref currWaypoints,
                    ref shortestWaypoints
                    ))
                    {
                    foundPath = true;
                }
            }

            // Recurse on the left cell
            if (CanMove(startPos.y, startPos.x - 1, mapArray)) {
                if (CalculatePathHelper(
                    mapArray,
                    new Vector2Int(startPos.x - 1, startPos.y),
                    endPos,
                    ref currWaypoints,
                    ref shortestWaypoints
                    )) {
                    foundPath = true;
                }
            }

            // Recurse on the right cell
            if (CanMove(startPos.y, startPos.x + 1, mapArray)) {
                if (CalculatePathHelper(
                   mapArray,
                   new Vector2Int(startPos.x + 1, startPos.y),
                   endPos,
                   ref currWaypoints,
                   ref shortestWaypoints
                   )) {
                    foundPath = true;
                }
            }

            // Recurse on the top cell
            if (CanMove(startPos.y - 1, startPos.x, mapArray)) {
                if (CalculatePathHelper(
                   mapArray,
                   new Vector2Int(startPos.x, startPos.y - 1),
                   endPos,
                   ref currWaypoints,
                   ref shortestWaypoints
                   )) {
                    foundPath = true;
                }
            }

            // Backtrack
            if (foundPath) {
                mapArray[startPos.y, startPos.x] = 0;
            }
            else {
                // path cannot be found down this cell, so treat as impassable
                mapArray[startPos.y, startPos.x] = 1;
            }
            currWaypoints.Remove(startPos);

            return foundPath;
        }
    }

    private bool CanMove(int y, int x, int[,] mapArray) {
        try {
            return mapArray[y, x] == 0;
        }
        catch (Exception e) {
            return false;
        }
    }

    private int[,] ConvertMapToArray(List<TileData.WalkType> canWalkOn) {

        m_map.CompressBounds(); // avoids tricky debugging issue with lopsided maps

        int mapX = m_map.size.x;
        int mapY = m_map.size.y;

        int[,] arr = new int[mapY, mapX];

        // for each tile in the tilemap, in the array place a:
            // 0 for tiles the given Oncomer can move through
            // 2 for tiles the given Oncomer cannot move through
        // (1 is reserved for marking a cell as "visited" in the shortest path algorithm)

        int rowIndex = 0;
        int colIndex = 0;
        for (int row = m_map.cellBounds.max.y - 1; row > m_map.cellBounds.min.y - 1; --row) {
            for (int col = m_map.cellBounds.min.x; col < m_map.cellBounds.max.x; ++col) {

                Vector3Int gridPos = new Vector3Int(col, row, 0);
                TileBase currTile = m_map.GetTile(gridPos);

                if (currTile == null) {
                    arr[rowIndex, colIndex] = 2;
                    colIndex++;
                    continue;
                }

                if (!m_tileDataDict.ContainsKey(currTile)) {
                    arr[rowIndex, colIndex] = 2;
                    colIndex++;
                    continue;
                }

                TileData.WalkType walkType = m_tileDataDict[currTile].GetWalkType();

                if (canWalkOn.Contains(walkType)) {
                    // set a 0
                    arr[rowIndex, colIndex] = 0;
                }
                else {
                    // set a 2
                    arr[rowIndex, colIndex] = 2;
                }
                colIndex++;
            }
            colIndex = 0;
            rowIndex++;
        }

        /* For debugging the array

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < mapY; i++) {
            for (int j = 0; j < mapX; j++) {
                sb.Append(arr[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());

        */

        return arr;
    }

    private List<Vector2> ConvertArrayPointsToMap(List<Vector2> waypoints) {
        List<Vector2> convertedPoints = new List<Vector2>();

        int adjustX = -m_map.cellBounds.min.x;
        int adjustY = m_map.cellBounds.max.y - 1;

        if (waypoints == null) {
            return null;
        }

        foreach (Vector2 point in waypoints) {
            Vector2 correctedPoint = new Vector2(point.x - adjustX, -(point.y - adjustY));

            convertedPoints.Add(correctedPoint);
        }

        return convertedPoints;
    }
}