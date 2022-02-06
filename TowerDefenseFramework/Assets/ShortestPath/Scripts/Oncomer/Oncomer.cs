using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Oncomer : MonoBehaviour {
    [SerializeField]
    private bool m_debugPath = false;
    [SerializeField]
    private GameObject m_debugPrefab;
    [SerializeField]
    private GameObject m_debugHolder;

    private List<TileData.WalkType> m_canWalkOn;

    private List<Vector2> m_waypoints;

    [SerializeField]
    private OncomerData m_oncomerData;

    private void Start() {
        ApplyOncomerData();

        CalculatePath();   
    }

    private void ApplyOncomerData() {
        this.GetComponent<SpriteRenderer>().sprite = m_oncomerData.Sprite;
        m_canWalkOn = m_oncomerData.CanWalkOn;
    }

    private void CalculatePath() {
        List<Vector2> tryWaypoints = TilemapManager.instance.CalculatePath(m_canWalkOn, this.transform.position);

        if (tryWaypoints == null) {
            Debug.Log("No possible path!");
            return;
        }

        m_waypoints = tryWaypoints;

        if (m_debugPath) {
            foreach (Vector2 waypoint in m_waypoints) {
                var debugWaypoint = Instantiate(m_debugPrefab, m_debugHolder.transform);
                debugWaypoint.transform.position = waypoint;
            }
        }
    }
}
