using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhNarwahl.pH;
using PhNarwahl;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Oncomer : MonoBehaviour, HasPh {
    public enum Type {
        Acid,
        Base,
    }

    private OncomerData m_oncomerData;

    private Oncomer.Type m_type;
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

    private static float WAYPOINT_BUFFER = 0.05f;

    public float getPH() {
        return pH.getPH(m_volume, m_molH, m_molOH);
    }

    public bool IsFull() {
        return m_volume >= m_volumeMax;
    }
    
    public int SpawnId { get; set; }
    private void Awake() {
        // Framework Case
        ApplyOncomerData(m_oncomerData);
    }

    // Used by Nexus when instantiating
    public void ManualAwake() {
        ApplyOncomerData(m_oncomerData);
    }

    private void Update() {
        MoveThroughPoints();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destination moat = other.gameObject.GetComponent<Destination>();
        if (moat != null) {
            moat.MixSolution(m_volume, m_molH, m_molOH);
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

    public void ApplyOncomerData(OncomerData oncomerData) {
        if(oncomerData == null) {
            return;
        }
        m_oncomerData = oncomerData;
        m_type = oncomerData.Type;
        GetComponent<SpriteRenderer>().sprite = oncomerData.Sprite;
        transform.Find("Sprite_Empty").GetComponent<SpriteRenderer>().sprite = oncomerData.EmptySprite;
        m_canWalkOn = oncomerData.CanWalkOn;
        m_speed = oncomerData.Speed;
        m_volume = oncomerData.StartingVolume;
        m_volumeMax = oncomerData.MaxVolume;
        UpdateVolumeMask();

        m_molH = pH.getAcidMolarity(oncomerData.StartingPh);
        m_molOH = pH.getBaseMolarity(oncomerData.StartingPh);
        m_movesDiagonal = oncomerData.MovesDiagonal;
        
        
        CalculatePath();
    }

    private void UpdateVolumeMask() {
        float fillPercent = m_volume / m_volumeMax;
        Transform spriteMask = gameObject.transform.Find("Sprite_Mask");
        float currentFillPercent = spriteMask.localScale.y;
        spriteMask.localScale += new Vector3(0, fillPercent - currentFillPercent, 0);
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
        UpdateVolumeMask();
    }

    private void CalculatePath() {
        m_currWaypointIndex = 0;
        List<Vector2> tryWaypoints = TilemapManager.instance.CalculatePath(m_canWalkOn, this.transform.position, m_movesDiagonal);

        if (tryWaypoints == null) {
            Debug.Log("No possible path!");
            return;
        }

        m_waypoints = tryWaypoints;
    }

    public Type GetOncomerType() {
        return m_type;
    }
}