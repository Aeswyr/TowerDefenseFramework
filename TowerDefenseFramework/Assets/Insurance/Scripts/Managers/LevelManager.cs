using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    enum GamePhase {
        Insurance, // purchase insurance
        Main // active gameplay
    }

    #region Level Data

    private float p_fire, p_storm, p_flood;
    private int n_butterflies;
    private TextAsset m_gridArrayTA;
    private int m_startFunds;
    private int m_quarterFunds;
    private float m_quarterTime;
    private float m_growthPerQuarter;
    private List<UIInsuranceMenu.Coverage> m_availableCoverages;

    #endregion // Level Data


    [SerializeField]
    private GameObject m_butterflyPrefab;
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
    private UIDeathMenu m_deathMenu;

    #region Editor Coverage

    [SerializeField]
    private UIInsuranceMenu m_insuranceMenu;

    // the coverage available in the level
    private Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> m_availableCoverageMap;

    // the coverage the player chooses to buy
    private List<UIInsuranceMenu.InsuranceType> m_insuranceSelections;
    private Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> m_currCoverageDict;

    #endregion // Editor Coverage

    // Debug
    [SerializeField]
    private GameObject m_oncomerPrefab;

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
        EventManager.OnDeath.AddListener(HandleDeath);

        AudioManager.instance.PlayAudio("lark", true);

        m_insuranceSelections = new List<UIInsuranceMenu.InsuranceType>();
    }

    private void Start() {
        // Load LevelData
        LoadLevelData(GameDB.instance.GetLevelData(GameManager.instance.CurrLevelID));

        // generate grid
        if (m_gridArrayTA != null) {
            TilemapManager.instance.LoadGridFromArray(m_gridArrayTA);
        }

        // set up text
        SetupText();

        // assign initial funds
        ModifyFunds(m_startFunds);

        // begin level at insurance phase
        m_phase = GamePhase.Insurance;
        m_insuranceMenu.Open();
    }

    private void LoadLevelData(LevelData data) {
        p_fire = data.PFire;
        p_storm = data.PStorm;
        p_flood = data.PFlood;
        n_butterflies = data.NumButterflies;
        m_startFunds = data.StartFunds;
        m_quarterFunds = data.QuarterFunds;
        m_gridArrayTA = data.GridArrayTA;
        m_quarterTime = data.QuarterTime;
        m_growthPerQuarter = data.QuarterGrowth;
        m_availableCoverages = data.AvailableCoverages;
    }

    private void SetupText() {
        if (p_fire > 0) {
            p_fireTransform = 1 - Mathf.Pow((1 - p_fire), 1.0f / n_butterflies);
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
    }

    private void Update() {
        if (GameManager.instance.IsPaused) {
            return;
        }

        GetDebugInputs();

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

            // Add funds
            ModifyFunds(m_quarterFunds);

            foreach (UIInsuranceMenu.InsuranceType key in m_currCoverageDict.Keys) {
                ModifyFunds(-(int)m_currCoverageDict[key].Premium);
            }
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

    public void DamageStation(float dmg, Oncomer.Type type) {
        m_station.ApplyDamage(dmg, type);
    }

    public void SetInsuranceSelections(List<UIInsuranceMenu.Coverage> insuranceSelections) {
        if (m_currCoverageDict == null) {
            m_currCoverageDict = new Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage>();
        }
        else {
            m_currCoverageDict.Clear();
        }

        foreach (UIInsuranceMenu.Coverage coverage in insuranceSelections) {
            m_currCoverageDict.Add(coverage.Type, coverage);
        }
    }

    public Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> GetInsuranceSelections() {
        return m_currCoverageDict;
    }

    #region Event Handlers

    void HandlePurchaseInsuranceComplete() {
        m_phase = GamePhase.Main;

        // Pay for insurance 
        foreach (UIInsuranceMenu.InsuranceType key in m_currCoverageDict.Keys) {
            ModifyFunds(-(int)m_currCoverageDict[key].Premium);
        }

        float floodInsuranceAmt = m_currCoverageDict.ContainsKey(UIInsuranceMenu.InsuranceType.Flood) ?
            m_currCoverageDict[UIInsuranceMenu.InsuranceType.Flood].MaxCoverage : 0;
        float fireInsuranceAmt = m_currCoverageDict.ContainsKey(UIInsuranceMenu.InsuranceType.Fire) ?
            m_currCoverageDict[UIInsuranceMenu.InsuranceType.Fire].MaxCoverage : 0;
        float stormInsuranceAmt = m_currCoverageDict.ContainsKey(UIInsuranceMenu.InsuranceType.Storm) ?
            m_currCoverageDict[UIInsuranceMenu.InsuranceType.Storm].MaxCoverage : 0;
        float umbrellaInsuranceAmt = m_currCoverageDict.ContainsKey(UIInsuranceMenu.InsuranceType.Umbrella) ?
    m_currCoverageDict[UIInsuranceMenu.InsuranceType.Umbrella].MaxCoverage : 0;

        m_station.InitHealth(50, floodInsuranceAmt, fireInsuranceAmt, stormInsuranceAmt, umbrellaInsuranceAmt);
    }

    void HandleDeath() {
        m_deathMenu.Open();
    }

    #endregion

    #region Debug

    private void GetDebugInputs() {
        if ((Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
            || (Input.GetKey(KeyCode.E) && Input.GetKeyDown(KeyCode.LeftShift))) {
            // Spawn one of each enemy
            InstantiateDebugOncomer(Nexus.Type.Deluvian);
            InstantiateDebugOncomer(Nexus.Type.FireSwathe);
            InstantiateDebugOncomer(Nexus.Type.Storm);
        }
    }

    private void InstantiateDebugOncomer(Nexus.Type nexusType) {
        GameObject oncomerObj = Instantiate(m_oncomerPrefab);
        oncomerObj.transform.position = TilemapManager.instance.GetNexusHubTransform(nexusType).position;
        Oncomer oncomer = oncomerObj.GetComponent<Oncomer>();

        switch (nexusType) {
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

    #endregion

    #region Coverage

    public UIInsuranceMenu.Coverage GetCoverage(UIInsuranceMenu.InsuranceType type) {
        // initialize the map if it does not exist
        if (instance.m_availableCoverageMap == null) {
            instance.m_availableCoverageMap = new Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage>();
            foreach (UIInsuranceMenu.Coverage c in instance.m_availableCoverages) {
                instance.m_availableCoverageMap.Add(c.Type, c);
            }
        }
        if (instance.m_availableCoverageMap.ContainsKey(type)) {
            return instance.m_availableCoverageMap[type];
        }
        else {
            throw new KeyNotFoundException(string.Format("No Coverage " +
                "with type `{0}' is in the level database of available coverages", type
            ));
        }
    }

    #endregion
}
