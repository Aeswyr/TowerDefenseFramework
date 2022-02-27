using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider m_baseSlider;
    [SerializeField]
    private Slider m_insuranceSlider;

    private float m_currBaseHealth, m_totalBaseHealth;
    private float m_currInsuranceHealth, m_totalInsuranceHealth;

    public void InitFields(float startBaseHealth, float startInsuranceHealth) {
        m_currBaseHealth = m_totalBaseHealth = startBaseHealth;
        m_currInsuranceHealth = m_totalInsuranceHealth = startInsuranceHealth;

        m_baseSlider.value = m_currBaseHealth > 0 ? 1 : 0;
        m_insuranceSlider.value = m_currInsuranceHealth > 0 ? 1 : 0;
    }

    public void ModifyHealth(float change) {
        m_currBaseHealth += change;
        if (m_currBaseHealth > m_totalBaseHealth) {
            // cannot go above max health
            m_currBaseHealth = m_totalBaseHealth;
        }
        else if (m_currBaseHealth < 0) {
            // cannot go below 0
            m_currBaseHealth = 0;

            // Trigger death / eat into insurance
        }

        // Update sliders
        m_baseSlider.value = m_currBaseHealth / m_totalBaseHealth;
        m_insuranceSlider.value = m_currInsuranceHealth / m_totalInsuranceHealth;
    }
}
