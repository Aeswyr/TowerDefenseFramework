using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameDB m_gameDB;

    private Dictionary<TileBase, TileData> m_tileDataDict;

    [SerializeField]
    private string m_outputFileName;

    [ContextMenu("Convert Grid to Array")]
    private void ConvertGridToArray() {
        Debug.Log("Converting");
        m_tileDataDict = m_gameDB.ConstructTileDataDict();
    }

    [SerializeField]
    private TextAsset m_inputTxt;

    [ContextMenu("Load Grid from Array")]
    private void LoadGridFromArray() {
        if (m_inputTxt == null) {
            Debug.Log("Failed to load grid from array: input text is null");
            return;
        }

        Debug.Log("Loading: " + m_inputTxt.text);
    }

}
