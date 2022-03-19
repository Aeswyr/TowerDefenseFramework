using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInsuranceMenu : MenuBase {
    public enum InsuranceType {
        Flood,
        Fire,
        Storm,
        Umbrella
    }

    [SerializeField] private Button m_selectFloodButton;
    [SerializeField] private Button m_selectFireButton;
    [SerializeField] private Button m_selectStormButton;
    [SerializeField] private Button m_selectUmbrellaButton;

    private List<Button> m_selectButtons;

    [SerializeField] private Button m_confirmButton;

    private List<InsuranceType> m_insuranceSelections;

    void OnEnable() {
        if (m_insuranceSelections == null) {
            m_insuranceSelections = new List<InsuranceType>();
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

    void HandleSelect(InsuranceType type) {
        if (m_insuranceSelections.Contains(type)) {
            m_insuranceSelections.Remove(type);
        }
        else {
            m_insuranceSelections.Add(type);
        }
    }

    void HandleSelectFlood() {
        HandleSelect(InsuranceType.Flood);

        // TODO: open reporting checklist
    }

    void HandleSelectFire() {
        HandleSelect(InsuranceType.Fire);

        // TODO: open reporting checklist
    }

    void HandleSelectStorm() {
        HandleSelect(InsuranceType.Storm);

        // TODO: open reporting checklist
    }

    void HandleSelectUmbrella() {
        HandleSelect(InsuranceType.Umbrella);

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
