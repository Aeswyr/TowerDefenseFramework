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
    private float m_totalWidth;
    private float m_baseLeftAnchorOffset;

    public void InitFields(float startBaseHealth, float startInsuranceHealth) {
        m_currBaseHealth = m_totalBaseHealth = startBaseHealth;
        m_currInsuranceHealth = m_totalInsuranceHealth = startInsuranceHealth;

        m_baseSlider.value = m_currBaseHealth > 0 ? 1 : 0;
        m_insuranceSlider.value = m_currInsuranceHealth > 0 ? 1 : 0;

        float totalHealth = startBaseHealth + startInsuranceHealth;
        m_insuranceSlider.value = m_currInsuranceHealth / totalHealth;

        /*
        // TODO: get this dynamically
        m_baseLeftAnchorOffset = 3;

        m_totalWidth = this.GetComponent<RectTransform>().rect.width;
        float baseOffset = m_insuranceSlider.fillRect.anchorMax.x * m_totalWidth - m_baseLeftAnchorOffset;
        m_baseSlider.transform.localScale = new Vector3(m_totalBaseHealth / totalHealth, 1, 1);
        m_baseSlider.transform.position = this.transform.position + new Vector3(baseOffset, 0, 0);
        //m_baseSlider.GetComponent<RectTransform>().anchorMax = new Vector2(1 - (baseOffset / m_totalWidth), 1);
        //m_baseSlider.GetComponent<RectTransform>().anchorMin = new Vector2(-(baseOffset / m_totalWidth), 0);
        //m_baseSlider.transform.position = this.transform.position + new Vector3(baseOffset, 0, 0);
        */

        //float baseOffset = m_insuranceSlider.fillRect.anchorMax.x * m_totalWidth - m_baseLeftAnchorOffset;
        //m_baseSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_totalWidth - baseOffset);
        //m_baseSlider.GetComponent<RectTransform>().sizeDelta.y);
    }

    void Update() {
        /*
        // Bring the left of base health flush to right of insurance health
        float baseOffset = m_insuranceSlider.fillRect.anchorMax.x * m_totalWidth - m_baseLeftAnchorOffset;
        m_baseSlider.transform.position = this.transform.position + new Vector3(baseOffset, 0, 0);
        //m_baseSlider.GetComponent<RectTransform>().anchorMax = new Vector2(1 - (baseOffset / m_totalWidth), 1);
        //m_baseSlider.GetComponent<RectTransform>().anchorMin = new Vector2(-(baseOffset / m_totalWidth), 0);
        //m_baseSlider.transform.position = this.transform.position + new Vector3(baseOffset, 0, 0);
        */
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
