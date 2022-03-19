using System;
using System.Collections;
using System.Collections.Generic;
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
        public float Premium;
        public float Deductible;
        public float MaxCoverage;

        public Coverage(InsuranceType type, float premium, float deductible, float maxCoverage) {
            Type = type;
            Premium = premium;
            Deductible = deductible;
            MaxCoverage = maxCoverage;
        }
    }

    [SerializeField] private Button m_selectFloodButton;
    [SerializeField] private Button m_selectFireButton;
    [SerializeField] private Button m_selectStormButton;
    [SerializeField] private Button m_selectUmbrellaButton;

    private List<Button> m_selectButtons;

    [SerializeField] private Button m_confirmButton;

    private List<Coverage> m_insuranceSelections;

    void OnEnable() {
        if (m_insuranceSelections == null) {
            m_insuranceSelections = new List<Coverage>();
        }
        if (m_selectButtons == null) {
            m_selectButtons = new List<Button>();

            m_selectButtons.Add(m_selectFloodButton);
            m_selectButtons.Add(m_selectFireButton);
            m_selectButtons.Add(m_selectStormButton);
            m_selectButtons.Add(m_selectUmbrellaButton);
        }

        m_selectFloodButton.onClick.AddListener(HandleSelectFlood);
        m_selectFireButton.onClick.AddListener(HandleSelectFire);
        m_selectStormButton.onClick.AddListener(HandleSelectStorm);
        m_selectUmbrellaButton.onClick.AddListener(HandleSelectUmbrella);
        m_confirmButton.onClick.AddListener(HandleConfirm);

        foreach (Button b in m_selectButtons) {
            b.onClick.AddListener(delegate { UpdateSelectColor(b); });
        }
    }

    void OnDisable() {
        m_selectFloodButton.onClick.RemoveAllListeners();
        m_selectFireButton.onClick.RemoveAllListeners();
        m_selectStormButton.onClick.RemoveAllListeners();
        m_selectUmbrellaButton.onClick.RemoveAllListeners();
        m_confirmButton.onClick.RemoveAllListeners();
    }

    public void Open() {
        base.OpenMenu();
    }

    #region Button Handlers

    void HandleSelect(Coverage coverage) {
        if (m_insuranceSelections.Contains(coverage)) {
            m_insuranceSelections.Remove(coverage);

            // check if umbrella insurance is still valid,
            if (m_insuranceSelections.Count == 1
                && m_insuranceSelections.Contains(LevelManager.instance.GetCoverage(InsuranceType.Umbrella))) {
                m_insuranceSelections.Clear();
                UpdateSelectColor(m_selectUmbrellaButton);
            }
        }
        else {
            m_insuranceSelections.Add(coverage);
        }
    }

    void HandleSelectFlood() {
        HandleSelect(LevelManager.instance.GetCoverage(InsuranceType.Flood));

        // TODO: open reporting checklist
    }

    void HandleSelectFire() {
        HandleSelect(LevelManager.instance.GetCoverage(InsuranceType.Fire));

        // TODO: open reporting checklist
    }

    void HandleSelectStorm() {
        HandleSelect(LevelManager.instance.GetCoverage(InsuranceType.Storm));

        // TODO: open reporting checklist
    }

    void HandleSelectUmbrella() {
        if (m_insuranceSelections.Count < 1) {
            UpdateSelectColor(m_selectUmbrellaButton);
        }
        else {
            HandleSelect(LevelManager.instance.GetCoverage(InsuranceType.Umbrella));
        }

        // TODO: open reporting checklist
    }

    void HandleConfirm() {
        base.CloseMenu();
        LevelManager.instance.SetInsuranceSelections(m_insuranceSelections);
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
}
