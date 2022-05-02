using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceManager : MonoBehaviour
{
    public static InsuranceManager Instance;

    [SerializeField] private Button m_optionsButton;
    [SerializeField] private UIInsuranceMenu m_optionsMenu;

    [SerializeField]
    private InsuranceSlider m_floodInsuranceSlider;
    [SerializeField]
    private InsuranceSlider m_fireInsuranceSlider;
    [SerializeField]
    private InsuranceSlider m_stormInsuranceSlider;
    [SerializeField]
    private InsuranceSlider m_umbrellaInsuranceSlider;

    private List<UIInsuranceMenu.Coverage> m_availableCoverages;

    // the coverage available in the level
    private Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> m_availableCoverageMap;

    // the coverage the player chooses to buy
    private List<UIInsuranceMenu.InsuranceType> m_insuranceSelections;
    private Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> m_currCoverageDict;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (this != Instance) {
            Destroy(this.gameObject);
            return;
        }

        m_insuranceSelections = new List<UIInsuranceMenu.InsuranceType>();

        m_optionsButton.onClick.AddListener(HandleOptions);

        // Event Handlers
        EventManager.OnPurchaseInsuranceComplete.AddListener(HandlePurchaseInsuranceComplete);
    }


    public void OpenOptionsMenu() {
        GameManager.instance.IsPaused = true;
        m_optionsMenu.Open();
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

    public void PayForNewCoverages() {
        foreach (UIInsuranceMenu.InsuranceType key in m_currCoverageDict.Keys) {
            // check radial timer
            if (GetSliderByType(key).Timer.TimeRemaining == 0) {
                // insurance is new
                // LevelManager.instance.AttemptPurchase((int)m_currCoverageDict[key].Premium);

                // TODO: start timer
                // GetSliderByType(key).Timer.StartTimer();
            }
            else {
                // still in effect, so no payment
            }
        }
    }

    public void PayForAllCoverages() {
        foreach (UIInsuranceMenu.InsuranceType key in m_currCoverageDict.Keys) {
            LevelManager.instance.AttemptPurchase((int)m_currCoverageDict[key].Premium);
        }
    }

    public float GetInsuranceAmount(UIInsuranceMenu.InsuranceType type) {
        return m_currCoverageDict.ContainsKey(type) ? m_currCoverageDict[type].MaxCoverage : 0;
    }

    public Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> GetInsuranceSelections() {
        return m_currCoverageDict;
    }

    public UIInsuranceMenu.Coverage GetCoverage(UIInsuranceMenu.InsuranceType type) {
        // initialize the map if it does not exist
        if (Instance.m_availableCoverageMap == null) {
            Instance.m_availableCoverageMap = new Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage>();
            foreach (UIInsuranceMenu.Coverage c in Instance.m_availableCoverages) {
                Instance.m_availableCoverageMap.Add(c.Type, c);
            }
        }
        if (Instance.m_availableCoverageMap.ContainsKey(type)) {
            return Instance.m_availableCoverageMap[type];
        }
        else {
            throw new KeyNotFoundException(string.Format("No Coverage " +
                "with type `{0}' is in the level database of available coverages", type
            ));
        }
    }

    public List<UIInsuranceMenu.Coverage> GetAvailableCoverages() {
        return m_availableCoverages;
    }

    public void SetAvailableCoverages(List<UIInsuranceMenu.Coverage> coverages) {
        m_availableCoverages = coverages;
    }

    public void InitSliderVals() {
        // init slider and radial values
        InitSliderHelper(m_floodInsuranceSlider, UIInsuranceMenu.InsuranceType.Flood);
        InitSliderHelper(m_fireInsuranceSlider, UIInsuranceMenu.InsuranceType.Fire);
        InitSliderHelper(m_stormInsuranceSlider, UIInsuranceMenu.InsuranceType.Storm);
        InitSliderHelper(m_umbrellaInsuranceSlider, UIInsuranceMenu.InsuranceType.Umbrella);
    }

    private void InitSliderHelper(InsuranceSlider iSlider, UIInsuranceMenu.InsuranceType type) {
        iSlider.Slider.value = iSlider.Timer.Radial.fillAmount = m_currCoverageDict.ContainsKey(type) ? 1 : 0;
    }

    public void ModifyInsuranceHealth(float floodVal, float fireVal, float stormVal, float umbrellaVal) {
        m_floodInsuranceSlider.Slider.value = floodVal;
        m_fireInsuranceSlider.Slider.value = fireVal;
        m_stormInsuranceSlider.Slider.value = stormVal;
        m_umbrellaInsuranceSlider.Slider.value = umbrellaVal;
    }

    private InsuranceSlider GetSliderByType(UIInsuranceMenu.InsuranceType type) {
        switch (type) {
            case UIInsuranceMenu.InsuranceType.Flood:
                return m_floodInsuranceSlider;
            case UIInsuranceMenu.InsuranceType.Fire:
                return m_fireInsuranceSlider;
            case UIInsuranceMenu.InsuranceType.Storm:
                return m_stormInsuranceSlider;
            case UIInsuranceMenu.InsuranceType.Umbrella:
                return m_umbrellaInsuranceSlider;
            default:
                return null;
        }
    }

    #region Event Handlers

    private void HandleOptions() {
        OpenOptionsMenu();
    }

    void HandlePurchaseInsuranceComplete() {
        // Pay for insurance
        PayForNewCoverages();
    }

    #endregion // Event Handlers
}
