using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhNarwahl.pH;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Oncomer : MonoBehaviour {
    public enum Type {
        // Framework
        Spider,
        Salamander,
        Truck
    }

    [SerializeField]
    private bool m_debugPath = false;
    [SerializeField]
    private GameObject m_debugPrefab;
    [SerializeField]
    private GameObject m_debugHolder;
    [SerializeField]
    private Type m_type; // only serialized for manual spawning

    private List<TileData.WalkType> m_canWalkOn;
    private List<Vector2> m_waypoints;
    private float m_speed;
    private bool m_movesDiagonal;
    private int m_currWaypointIndex;

   //pH variables
    private float m_molOH;
    private float m_molH;
    private float m_volume;
    private float m_volumeMax;

    public OncomerData OncomerData {
        get; set;
    }

    private static float WAYPOINT_BUFFER = 0.05f;

    public float getPH() {
        return pH.getPH(m_volume, m_molH, m_molOH);
    }
    
    private void Awake() {
        if (m_type == Type.Spider || m_type == Type.Salamander) {
            // Framework Case
            Debug.Log("Framework spawning");
            this.OncomerData = GameDB.instance.GetOncomerData(m_type);

            ApplyOncomerData();

            CalculatePath();
        }
    }

    // Used by Nexus when instantiating
    public void ManualAwake() {
        ApplyOncomerData();

        CalculatePath();
    }

    private void Update() {
        MoveThroughPoints();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "moat") {
            other.gameObject.GetComponent<Moat>().MixSolution(m_volume, m_molH, m_molOH);
            Destroy(this.gameObject);
        }
    }

    private void MoveThroughPoints() {
        if (m_waypoints == null) {
            return;
        }

        if (m_currWaypointIndex < m_waypoints.Count) {
            Vector2 currPoint = m_waypoints[m_currWaypointIndex];
            MoveToward(currPoint);
        }
        else {
            // Reached destination (should have been destroyed on collision earlier, however)
            Debug.Log("Warning: Oncomer did not collide with target");
            Destroy(this.gameObject);
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
        m_type = this.OncomerData.Type;
        GetComponent<SpriteRenderer>().sprite = this.OncomerData.Sprite;
        m_canWalkOn = this.OncomerData.CanWalkOn;
        m_speed = this.OncomerData.Speed;
        m_volume = this.OncomerData.StartingVolume;
        m_volumeMax = this.OncomerData.MaxVolume;
        m_molH = pH.getAcidMolarity(this.OncomerData.StartingPh);
        m_molOH = pH.getBaseMolarity(this.OncomerData.StartingPh);
        m_movesDiagonal = this.OncomerData.MovesDiagonal;
    }

    public void MixSolution(float volume, float molH, float molOH) {
        float roomLeft = m_volumeMax - m_volume;
        if(roomLeft <= 0) {
            return;
        }

        float percentMixed = Mathf.Min(1, roomLeft / volume);
        
        m_volume += percentMixed * volume;
        m_molH += percentMixed * molH;
        m_molOH += percentMixed * molOH;
    }

    private void CalculatePath() {
        m_currWaypointIndex = 0;
        List<Vector2> tryWaypoints = TilemapManager.instance.CalculatePath(m_canWalkOn, this.transform.position, m_movesDiagonal);

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

    public Type GetOncomerType() {
        return m_type;
    }
}