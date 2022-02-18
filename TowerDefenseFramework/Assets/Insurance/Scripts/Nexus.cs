using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Nexus : MonoBehaviour
{
    public enum Type {
        Storm,
        FireSwathe,
        Deluvian
    }

    private enum State {
        Incubating,
        Returning,
        Releasing,
        Dissolving
    }

    [SerializeField]
    private Type m_type;
    [SerializeField]
    private float m_scaleReduction;
    [SerializeField]
    private float m_incubateSpeed;
    [SerializeField]
    private float m_returnSpeed;
    [SerializeField]
    private GameObject m_oncomerPrefab;

    private float m_growthRate;
    private float m_incubationTime;

    private float m_size;
    private float m_incubationTimer;

    private State m_state;
    private Vector3 m_returnPos;

    private SpriteRenderer m_sr;

    private void Update() {
        if (GameManager.instance.IsPaused) {
            return;
        }

        if (m_state == State.Incubating) {
            m_incubationTimer -= Time.deltaTime;
            m_size += Time.deltaTime * m_growthRate;
            this.transform.localScale = new Vector3((m_size + 1) / m_scaleReduction, (m_size + 1) / m_scaleReduction, 1);

            if (m_incubationTimer <= 0) {
                Return();
            }
        }
        else if (m_state == State.Returning) {
            Vector3 travelVector = (m_returnPos - this.transform.position);
            
            if (travelVector.magnitude <= .05f) {
                this.transform.Translate(travelVector);
                Release();
            }
            else {
                Vector3 dir = travelVector.normalized;
                this.transform.Translate(dir * m_returnSpeed * Time.deltaTime);
            }
        }
    }

    public void SetType(Nexus.Type type) {
        m_type = type;
    }

    public void ManualAwake() {
        m_sr = this.GetComponent<SpriteRenderer>();

        ApplyNexusData();

        m_incubationTimer = m_incubationTime;

        m_size = 1;

        m_state = State.Incubating;
    }

    private void ApplyNexusData() {
        NexusData data = GameDB.instance.GetNexusData(m_type);
        m_growthRate = data.GrowthRate;
        m_incubationTime = data.IncubationTime;
        m_sr.color = data.Color;
    }

    private void Return() {
        m_state = State.Returning;

        // TODO: move toward nexusHub
        m_returnPos = TilemapManager.instance.GetNexusHubTransform(m_type).position;
    }

    private void Release() {
        int numSpawns = 4;
        for (int i = 0; i < numSpawns; ++i) {

            GameObject oncomerObj = Instantiate(m_oncomerPrefab);
            Vector3 spawnSpread = new Vector3(-0.4f + .2f * i, 0, 0);
            oncomerObj.transform.position = this.transform.position + spawnSpread;
            Oncomer oncomer = oncomerObj.GetComponent<Oncomer>();

            switch (m_type) {
                case Nexus.Type.Storm:
                    oncomer.OncomerData = GameDB.instance.GetOncomerData(Oncomer.Type.Tempest);
                    break;
                case Nexus.Type.Deluvian:
                    oncomer.OncomerData = GameDB.instance.GetOncomerData(Oncomer.Type.Flood);
                    break;
                case Nexus.Type.FireSwathe:
                    oncomer.OncomerData = GameDB.instance.GetOncomerData(Oncomer.Type.Wildfire);
                    break;
                default:
                    Debug.Log("Unknown type of nexus. Unable to spawn oncomer.");
                    Destroy(oncomerObj);
                    break;
            }

            oncomer.ManualAwake();
        }

        Destroy(this.gameObject);
    }
}
