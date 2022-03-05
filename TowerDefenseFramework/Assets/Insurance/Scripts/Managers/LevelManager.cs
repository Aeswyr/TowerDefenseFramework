using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    enum GamePhase {
        Insurance, // purchase insurance
        Main // active gameplay
    }

    [SerializeField]
    private float p_fire, p_storm, p_flood;
    [SerializeField]
    private int n_butterflies;

    [SerializeField]
    private GameObject m_butterflyPrefab;
    [SerializeField]
    private float m_quarterTime;
    [SerializeField]
    private float m_growthPerQuarter;
    [SerializeField]
    private TextMeshProUGUI[] m_forecastTexts;
    [SerializeField]
    private TextMeshProUGUI m_periodText;
    [SerializeField]
    private TextMeshProUGUI m_periodTimerText;
    [SerializeField]
    private TextMeshProUGUI m_fundsText;
    [SerializeField]
    private Station m_station;
    [SerializeField]
    private GameObject m_insuranceMenu;

    private GamePhase m_phase;

    private float p_fireTransform, p_stormTransform, p_floodTransform;
    private float m_quarterTimer;
    private float m_butterflyTime;
    private float m_butterflyTimer;

    private int m_quarter;
    private float m_adjustedGrowth;
    private int m_funds;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (this != instance) {
            Debug.Log("Warning: multiple LevelManagers in the same scene. Undefined behavior may result.");
        }

        // Event Handlers
        EventManager.OnPurchaseInsuranceComplete.AddListener(HandlePurchaseInsuranceComplete);

        AudioManager.instance.PlayAudio("lark", true);
    }

    private void Start() {
        if (p_fire > 0) {
            p_fireTransform = 1 - Mathf.Pow((1-p_fire), 1.0f/n_butterflies);
        }
        else {
            p_fireTransform = 0;
        }
        if (p_storm > 0) {
            p_stormTransform = 1 - Mathf.Pow((1 - p_storm), 1.0f / n_butterflies);
        }
        else {
            p_stormTransform = 0;
        }
        if (p_flood > 0) {
            p_floodTransform = 1 - Mathf.Pow((1 - p_flood), 1.0f / n_butterflies);
        }
        else {
            p_floodTransform = 0;
        }

        m_quarterTimer = m_quarterTime;
        m_butterflyTime = m_quarterTime / n_butterflies;
        m_butterflyTimer = m_butterflyTime;

        m_forecastTexts[0].text = "Hurricane: " + (p_storm * 100) + "%";
        m_forecastTexts[1].text = "Wildfire: " + (p_fire * 100) + "%";
        m_forecastTexts[2].text = "Flood: " + (p_flood * 100) + "%";

        m_quarter = 0;
        m_periodText.text = "Period: 1";
        m_periodTimerText.text = m_quarterTime.ToString("F1") + " s";
        m_adjustedGrowth = 1;

        ModifyFunds(80);

        m_station.InitHealth(200, 0);

        m_phase = GamePhase.Insurance;
        m_insuranceMenu.SetActive(true);
    }

    private void Update() {
        if (GameManager.instance.IsPaused) {
            return;
        }

        switch (m_phase) {
            case GamePhase.Insurance:
                UpdateInsurancePhase();
                break;
            case GamePhase.Main:
                UpdateMainPhase();
                break;
            default:
                break;
        }
    }

    private void UpdateMainPhase() {
        m_quarterTimer -= Time.deltaTime;
        if (m_quarterTimer <= 0) {
            // End Quarter
            m_quarter++;
            m_periodText.text = "Period: " + (m_quarter + 1);
            m_adjustedGrowth = 1 + m_quarter * m_growthPerQuarter;
            m_quarterTimer = m_quarterTime;

            // Add funds (Hack)
            ModifyFunds(35);

            // Pay for insurance (Hack)
            ModifyFunds(-5);
        }
        m_periodTimerText.text = m_quarterTimer.ToString("F1") + " s";

        m_butterflyTimer -= Time.deltaTime;
        if (m_butterflyTimer <= 0) {
            m_butterflyTimer = m_butterflyTime;

            // fire
            GameObject butterfly = Instantiate(m_butterflyPrefab);
            NexusButterfly nexusB = butterfly.GetComponent<NexusButterfly>();
            nexusB.GetComponent<SpriteRenderer>().color = GameDB.instance.GetNexusColor(Nexus.Type.FireSwathe);
            nexusB.SetFields(p_fireTransform, Nexus.Type.FireSwathe, m_adjustedGrowth);
            nexusB.ManualAwake();

            // flood
            butterfly = Instantiate(m_butterflyPrefab);
            nexusB = butterfly.GetComponent<NexusButterfly>();
            nexusB.GetComponent<SpriteRenderer>().color = GameDB.instance.GetNexusColor(Nexus.Type.Deluvian);
            nexusB.SetFields(p_floodTransform, Nexus.Type.Deluvian, m_adjustedGrowth);
            nexusB.ManualAwake();

            // tempest
            butterfly = Instantiate(m_butterflyPrefab);
            nexusB = butterfly.GetComponent<NexusButterfly>();
            nexusB.GetComponent<SpriteRenderer>().color = GameDB.instance.GetNexusColor(Nexus.Type.Storm);
            nexusB.SetFields(p_stormTransform, Nexus.Type.Storm, m_adjustedGrowth);
            nexusB.ManualAwake();
        }
    }

    private void OnDestroy() {
        // Event Handlers
        EventManager.OnPurchaseInsuranceComplete.RemoveListener(HandlePurchaseInsuranceComplete);
    }

    private void UpdateInsurancePhase() {

    }

    public bool CheckFunds(int cost) {
        return cost <= m_funds;
    }

    public bool AttemptPurchase(int cost) {
        if (cost > m_funds) {
            return false;
        }

        ModifyFunds(-cost);
        return true;
    }

    private void ModifyFunds(int change) {
        m_funds += change;
        m_fundsText.text = "$" + m_funds;
    }

    public void DamageStation(float dmg) {
        m_station.ApplyDamage(dmg);
    }

    #region Event Handlers

    void HandlePurchaseInsuranceComplete() {
        m_phase = GamePhase.Main;

        // Pay for insurance (Hack)
        ModifyFunds(-5);
    }

    #endregion

}
