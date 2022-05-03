using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInsuranceMenu : MenuBase {
    public enum InsuranceType {
        Flood,
        Fire,
        Storm,
        Umbrella,
        None
    }

    [Serializable]
    public struct Coverage {
        public InsuranceType Type;
        [TextArea] public string Title;
        public float Premium;
        public float Deductible;
        public float MaxCoverage;
        [HideInInspector] public bool AutoRenew;

        public Coverage(InsuranceType type, string title, float premium, float deductible, float maxCoverage, bool autoRenew) {
            Type = type;
            Title = title;
            Premium = premium;
            Deductible = deductible;
            MaxCoverage = maxCoverage;
            AutoRenew = autoRenew;
        }
    }

    #region Editor Button Generator

    [SerializeField]
    private float m_colSpacing;
    [SerializeField]
    private float m_rowSpacing;
    [SerializeField]
    private int m_numCols;

    [SerializeField] private GameObject m_insuranceButtonPrefab;
    [SerializeField] private GameObject m_buttonHolder;

    private List<GameObject> m_buttonObjs;

    #endregion // Editor Button Generator

    private Button m_selectUmbrellaButton;

    private List<Button> m_selectButtons;

    [SerializeField] private Button m_confirmButton;
    [SerializeField] private TextMeshProUGUI m_detailsText;

    private List<Coverage> m_coveragesAvailable; // the options to choose from
    private List<Coverage> m_insuranceSelections; // the options chosen

    void OnEnable() {
        if (LevelManager.instance == null) {
            base.CloseMenu();
            return;
        }
        if (m_buttonObjs != null) {
            Cleanup();
        }

        if (m_insuranceSelections == null) {
            m_insuranceSelections = new List<Coverage>();
        }
        else {
            m_insuranceSelections.Clear();
        }

        GenerateButtons();

        if (m_selectButtons == null) {
            m_selectButtons = new List<Button>();
        }
        else {
            m_selectButtons.Clear();
        }
        foreach (GameObject buttonObj in m_buttonObjs) {
            m_selectButtons.Add(buttonObj.GetComponent<Button>());
        }

        m_confirmButton.onClick.AddListener(HandleConfirm);

        foreach (Button b in m_selectButtons) {
            b.onClick.AddListener(delegate { UpdateSelectColor(b); });
        }
        if (m_selectButtons.Count > 0) {
            m_detailsText.text = "Win-surance Coverage";
        }
        else {
            m_detailsText.text = "No Providers Available";
        }
    }

    void OnDisable() {
        if (LevelManager.instance != null) {
            Cleanup();
        }

        GameManager.instance.IsPaused = false;
    }

    public void Open() {
        base.OpenMenu();
    }

    #region Button Handlers

    void HandleSelect(int index) {
        Coverage coverage = m_coveragesAvailable[index];
        if (m_insuranceSelections.Contains(coverage)) {

            m_insuranceSelections.Remove(coverage);

            // check if umbrella insurance is still valid,
            if (m_selectUmbrellaButton != null 
                && m_insuranceSelections.Count == 1
                && m_insuranceSelections.Contains(InsuranceManager.Instance.GetCoverage(InsuranceType.Umbrella))) {
                m_insuranceSelections.Clear();
                UpdateSelectColor(m_selectUmbrellaButton);
            }
        }
        else {
            // if umbrella, check that there is at least one other selected
            if (coverage.Type == InsuranceType.Umbrella) {
                if (m_selectUmbrellaButton != null
                    && m_insuranceSelections.Count < 1) {
                    UpdateSelectColor(m_selectUmbrellaButton);
                }
                else {
                    m_insuranceSelections.Add(coverage);
                }
            }
            // add normally
            else {
                m_insuranceSelections.Add(coverage);
            }
        }
    }

    void HandleSelectRenew(int index) {
        Coverage tempCoverage = m_coveragesAvailable[index];
        tempCoverage.AutoRenew = !tempCoverage.AutoRenew;
        m_coveragesAvailable[index] = tempCoverage;

        for (int i = 0; i < m_insuranceSelections.Count; i++) {
            if (m_insuranceSelections[i].Title == tempCoverage.Title) {
                Coverage tempSelection = m_insuranceSelections[i];
                tempSelection.AutoRenew = tempCoverage.AutoRenew;
                m_insuranceSelections[i] = tempSelection;
            }
        }
    }

    void HandleConfirm() {
        base.CloseMenu();
        InsuranceManager.Instance.SetInsuranceSelections(m_insuranceSelections);
        EventManager.OnPurchaseInsuranceComplete.Invoke();
    }

    void UpdateSelectColor(Button b) {
        Image bImage = b.GetComponent<Image>();

        // TODO: specify and load these colors externally
        if (bImage.color == Color.green) {
            bImage.color = Color.white;
        }
        else {
            bImage.color = Color.green;
        }
    }

    #endregion // Button Handlers


    #region Button Generator

    private void GenerateButtons() {
        if (m_buttonObjs == null) {
            m_buttonObjs = new List<GameObject>();
        }
        else {
            Cleanup();
        }

        if (m_coveragesAvailable == null) {
            m_coveragesAvailable = InsuranceManager.Instance.GetAvailableCoverages();
        }

        int colIndex = 0;
        for (int i = 0; i < m_coveragesAvailable.Count; i++) {
            Coverage coverage = m_coveragesAvailable[i];

            // instantiate button
            GameObject insuranceButtonObj = Instantiate(m_insuranceButtonPrefab, m_buttonHolder.transform);

            // set spacing
            float horizSpacing = (colIndex % m_numCols) * m_colSpacing;
            float vertSpacing = (colIndex / m_numCols) * -m_rowSpacing;
            insuranceButtonObj.transform.position += new Vector3(horizSpacing, vertSpacing, 0);

            // assign scene and text
            Button insuranceBaseButton = insuranceButtonObj.GetComponent<Button>();
            int tempI = i;
            insuranceBaseButton.onClick.AddListener(delegate { HandleSelect(tempI); });
            InsuranceButton insuranceButton = insuranceButtonObj.GetComponent<InsuranceButton>();
            insuranceButton.SetText(coverage.Title, ("" + coverage.Premium), ("" + coverage.Deductible));

            // restore existing auto-renew settings
            if (coverage.AutoRenew) {
                insuranceButton.SetAutoRenew(true);
            }

            insuranceButton.RenewToggle.onValueChanged.AddListener(delegate { HandleSelectRenew(tempI); });

            // set green if already selected from previous round
            if (InsuranceManager.Instance.CoverageIsActive(coverage)) {
                m_insuranceSelections.Add(coverage);
                UpdateSelectColor(insuranceBaseButton);
            }

            // save to buttons
            m_buttonObjs.Add(insuranceButtonObj);
            if (coverage.Type == InsuranceType.Umbrella) {
                m_selectUmbrellaButton = insuranceBaseButton;
            }

            // move to next column
            ++colIndex;
        }
    }

    private void Cleanup() {
        foreach (GameObject obj in m_buttonObjs) {
            Destroy(obj);
        }
        m_buttonObjs.Clear();
    }

    #endregion
}
