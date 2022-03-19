using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    [SerializeField] bool DEBUGGING = false;

    [SerializeField]
    private Slider m_baseSlider;
    [SerializeField]
    private Slider m_insuranceSlider;

    private float m_currBaseHealth, m_totalBaseHealth;
    private float m_currInsuranceHealth, m_totalInsuranceHealth;
    private float m_totalWidth;
    private float m_baseLeftAnchorOffset;

    private Dictionary<UIInsuranceMenu.InsuranceType, UIInsuranceMenu.Coverage> m_currCoveragesDict;

    public void InitFields(float startBaseHealth, float startInsuranceHealth) {
        m_currBaseHealth = m_totalBaseHealth = startBaseHealth;
        m_currInsuranceHealth = m_totalInsuranceHealth = startInsuranceHealth;

        m_baseSlider.value = m_currBaseHealth > 0 ? 1 : 0;
        m_insuranceSlider.value = m_currInsuranceHealth > 0 ? 1 : 0;

        //float totalHealth = startBaseHealth + startInsuranceHealth;
        //m_insuranceSlider.value = m_currInsuranceHealth / totalHealth;
    }

    public void ModifyHealth(float change, Oncomer.Type type) {
        if (change < 0) {
            DamageHealth(-change, type);
        }
        else {

        }

        // Update sliders
        m_baseSlider.value = m_currBaseHealth / m_totalBaseHealth;
        m_insuranceSlider.value = m_currInsuranceHealth / m_totalInsuranceHealth;
    }

    #region Helper Methods

    private void DamageHealth(float dmg, Oncomer.Type type) {
        m_currCoveragesDict = LevelManager.instance.GetInsuranceSelections();

        // divvy up damage according to insurance
        switch (type) {
            case Oncomer.Type.Flood:
                CoverageSequence(UIInsuranceMenu.InsuranceType.Flood, dmg);
                break;
            case Oncomer.Type.Tempest:
                CoverageSequence(UIInsuranceMenu.InsuranceType.Storm, dmg);
                break;
            case Oncomer.Type.Wildfire:
                CoverageSequence(UIInsuranceMenu.InsuranceType.Fire, dmg);
                break;
            case Oncomer.Type.None:
                //TODO: test this case?
                CoverageSequence(UIInsuranceMenu.InsuranceType.None, dmg);
                break;
            default:
                break;
        }
    }

    private void CoverageSequence(UIInsuranceMenu.InsuranceType insuranceType, float dmg) {
        if (DEBUGGING) { Debug.Log("Starting coverage sequence for " + insuranceType + " of damage " + dmg); }
        if (DEBUGGING) { Debug.Log("Checking for relevant insurance..."); }

        float fullDmg = dmg;
        if (insuranceType != UIInsuranceMenu.InsuranceType.Umbrella && m_currCoveragesDict.ContainsKey(insuranceType)) {
            if (DEBUGGING) { Debug.Log("Has relevant coverage: " + insuranceType); }
            UIInsuranceMenu.Coverage relevantCoverage = m_currCoveragesDict[insuranceType];

            if (DEBUGGING) { Debug.Log("Applying deductible of value " + relevantCoverage.Deductible); }
            // apply deductible to base health
            float deductibleAmt = Mathf.Min(relevantCoverage.Deductible, dmg);
            dmg -= deductibleAmt;
            dmg += TryCoverDamage(deductibleAmt, UIInsuranceMenu.InsuranceType.None);
            if (DEBUGGING) { Debug.Log("Remaining damage: " + dmg); }
            if (dmg <= 0) { return; }

            // apply as much as possible to relevant insurance
            if (DEBUGGING) { Debug.Log("applying remaining " + dmg + " dmg to insurance of type " + insuranceType); }
            fullDmg = dmg;
            dmg = 0;
            dmg += TryCoverDamage(fullDmg, insuranceType);
            if (DEBUGGING) { Debug.Log("Remaining damage: " + dmg); }
            if (dmg <= 0) { return; }
        }

        if (DEBUGGING) { Debug.Log("Checking for umbrella insurance..."); }

        if (m_currCoveragesDict.ContainsKey(UIInsuranceMenu.InsuranceType.Umbrella)) {
            if (DEBUGGING) { Debug.Log("Has umbrella insurance"); }
            UIInsuranceMenu.Coverage umbrellaCoverage = m_currCoveragesDict[UIInsuranceMenu.InsuranceType.Umbrella];

            // apply umbrella deductible to base health
            if (DEBUGGING) { Debug.Log("Applying deductible of value " + umbrellaCoverage.Deductible); }
            float deductibleAmt = Mathf.Min(umbrellaCoverage.Deductible, dmg);
            dmg -= deductibleAmt;
            dmg += TryCoverDamage(deductibleAmt, UIInsuranceMenu.InsuranceType.None);
            if (DEBUGGING) { Debug.Log("Remaining damage: " + dmg); }
            if (dmg <= 0) { return; }

            // apply rest to umbrella insurance
            if (DEBUGGING) { Debug.Log("applying remaining " + dmg + " dmg to insurance of type Umbrella"); }
            fullDmg = dmg;
            dmg = 0;
            dmg += TryCoverDamage(fullDmg, UIInsuranceMenu.InsuranceType.Umbrella);
            if (DEBUGGING) { Debug.Log("Remaining damage: " + dmg); }
            if (dmg <= 0) { return; }
        }

        // apply remainder to base health
        if (DEBUGGING) { Debug.Log("applying remaining " + dmg + " dmg to insurance of type None (directly to base"); }
        fullDmg = dmg;
        dmg = 0;
        dmg += TryCoverDamage(fullDmg, UIInsuranceMenu.InsuranceType.None);
        if (DEBUGGING) { Debug.Log("Remaining damage: " + dmg); }

        if (DEBUGGING) { Debug.Log("End sequence-------------------"); }

        return;
    }

    private float TryCoverDamage(float dmg, UIInsuranceMenu.InsuranceType insuranceType) {
        float remainder = 0;
        switch (insuranceType) {
            case UIInsuranceMenu.InsuranceType.Fire:
                // TODO: assign to fire-specific insurance
                m_currInsuranceHealth -= dmg;
                if (m_currInsuranceHealth < 0) {
                    // store remainder
                    remainder = -m_currInsuranceHealth;
                    // set insurance health to 0
                    m_currInsuranceHealth = 0;
                }
                break;
            case UIInsuranceMenu.InsuranceType.Flood:
                // TODO: assign to flood-specific insurance
                m_currInsuranceHealth -= dmg;
                if (m_currInsuranceHealth < 0) {
                    // store remainder
                    remainder = -m_currInsuranceHealth;
                    // set insurance health to 0
                    m_currInsuranceHealth = 0;
                }
                break;
            case UIInsuranceMenu.InsuranceType.Storm:
                // TODO: assign to storm-specific insurance
                m_currInsuranceHealth -= dmg;
                if (m_currInsuranceHealth < 0) {
                    // store remainder
                    remainder = -m_currInsuranceHealth;
                    // set insurance health to 0
                    m_currInsuranceHealth = 0;
                }
                break;
            case UIInsuranceMenu.InsuranceType.Umbrella:
                m_currInsuranceHealth -= dmg;
                if (m_currInsuranceHealth < 0) {
                    // store remainder
                    remainder = -m_currInsuranceHealth;
                    // set insurance health to 0
                    m_currInsuranceHealth = 0;
                }
                break;
            case UIInsuranceMenu.InsuranceType.None:
                m_currBaseHealth -= dmg;
                if (m_currBaseHealth <= 0) {
                    // set base health to 0
                    m_currBaseHealth = 0;

                    // no coverage left means death.
                    EventManager.OnDeath.Invoke();

                    Debug.Log("Station destroyed");

                    // no remainder because level is lost
                }
                break;
            default:
                break;
        }

        Debug.Log("Remainder: " + remainder);
        return remainder;
    }

    #endregion // Helper Methods
}
