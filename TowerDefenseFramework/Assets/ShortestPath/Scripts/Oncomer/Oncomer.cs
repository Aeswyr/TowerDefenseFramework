using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Oncomer : MonoBehaviour {
    [SerializeField]
    private bool m_debugPath = false;
    [SerializeField]
    private GameObject m_debugPrefab;
    [SerializeField]
    private GameObject m_debugHolder;

    private List<TileData.WalkType> m_canWalkOn;
    private List<Vector2> m_waypoints;
    private float m_speed;
    private int m_currWaypointIndex;

    [SerializeField]
    private OncomerData m_oncomerData;

    private static float WAYPOINT_BUFFER = 0.05f;

    private void Start() {
        ApplyOncomerData();

        CalculatePath();
    }

    private void Update() {
        MoveThroughPoints();
    }

    private void MoveThroughPoints() {
        if (m_waypoints == null) {
            return;
        }

        if (m_currWaypointIndex < m_waypoints.Count) {
            Vector2 currPoint = m_waypoints[m_currWaypointIndex];
            MoveToward(currPoint);
        }
    }

    private void MoveToward(Vector2 point) {
        Vector2 distance = (point - (Vector2)this.transform.position);
        Vector2 dir = (point - (Vector2)this.transform.position).normalized;

        if (distance.magnitude > WAYPOINT_BUFFER) {
            this.transform.Translate(dir * m_speed * Time.deltaTime);
            distance = (point - (Vector2)this.transform.position);
        }
        else {
            // move the rest of the way
            this.transform.Translate(distance);

            // increment to the next waypoint
            m_currWaypointIndex++;
        }
    }

    private void ApplyOncomerData() {
        this.GetComponent<SpriteRenderer>().sprite = m_oncomerData.Sprite;
        m_canWalkOn = m_oncomerData.CanWalkOn;
        m_speed = m_oncomerData.Speed;
    }

    private void CalculatePath() {
        m_currWaypointIndex = 0;
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
            m_debugHolder.transform.parent = null;
        }
    }

    #region Projectile

    public void ApplyProjectileEffects() {
        Debug.Log("Target was hit by a projectile!");
    }

    #endregion
}
