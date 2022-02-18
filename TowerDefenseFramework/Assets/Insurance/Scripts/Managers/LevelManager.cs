using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private float p_fire, p_storm, p_flood;
    [SerializeField]
    private int n_butterflies;

    [SerializeField]
    private GameObject m_butterflyPrefab;
    [SerializeField]
    private float m_quarterTime;

    private float p_fireTransform, p_stormTransform, p_floodTransform;
    private float m_quarterTimer;
    private float m_butterflyTime;
    private float m_butterflyTimer;

    private void Start() {
        if (p_fire > 0) {
            p_fireTransform = 1 - Mathf.Pow((1-p_fire), 1.0f/n_butterflies); //Mathf.Pow(2, Mathf.Log((1 - p_fire), 2) / n_butterflies);
        }
        else {
            p_fireTransform = 0;
        }
        if (p_storm > 0) {
            p_stormTransform = 1 - Mathf.Pow((1 - p_storm), 1.0f / n_butterflies); //Mathf.Pow(2, Mathf.Log((1 - p_storm), 2) / n_butterflies);
        }
        else {
            p_stormTransform = 0;
        }
        if (p_flood > 0) {
            p_floodTransform = 1 - Mathf.Pow((1 - p_flood), 1.0f / n_butterflies); //Mathf.Pow(2, Mathf.Log((1 - p_flood), 2) / n_butterflies);
        }
        else {
            p_floodTransform = 0;
        }

        m_quarterTimer = m_quarterTime;
        m_butterflyTime = m_quarterTime / n_butterflies;
        m_butterflyTimer = m_butterflyTime;
    }

    private void Update() {
        if (GameManager.instance.IsPaused) {
            return;
        }

        m_quarterTimer -= Time.deltaTime;
        if (m_quarterTimer <= 0) {
            // End Quarter
        }

        m_butterflyTimer -= Time.deltaTime;
        if (m_butterflyTimer <= 0) {
            m_butterflyTimer = m_butterflyTime;

            // fire
            GameObject butterfly = Instantiate(m_butterflyPrefab);
            NexusButterfly nexusB = butterfly.GetComponent<NexusButterfly>();
            nexusB.GetComponent<SpriteRenderer>().color = GameDB.instance.GetNexusColor(Nexus.Type.FireSwathe);
            nexusB.SetFields(p_fireTransform, Nexus.Type.FireSwathe);
            nexusB.ManualAwake();

            // flood
            butterfly = Instantiate(m_butterflyPrefab);
            nexusB = butterfly.GetComponent<NexusButterfly>();
            nexusB.GetComponent<SpriteRenderer>().color = GameDB.instance.GetNexusColor(Nexus.Type.Deluvian);
            nexusB.SetFields(p_floodTransform, Nexus.Type.Deluvian);
            nexusB.ManualAwake();

            // tempest
            butterfly = Instantiate(m_butterflyPrefab);
            nexusB = butterfly.GetComponent<NexusButterfly>();
            nexusB.GetComponent<SpriteRenderer>().color = GameDB.instance.GetNexusColor(Nexus.Type.Storm);
            nexusB.SetFields(p_stormTransform, Nexus.Type.Storm);
            nexusB.ManualAwake();
        }
    }

}
