using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private InsuranceTimer timer;
    [SerializeField] private UIInsuranceMenu.InsuranceType m_type;


    public Slider Slider {
        get { return slider; }
    }
    public InsuranceTimer Timer {
        get { return timer; }
    }
    public UIInsuranceMenu.InsuranceType Type {
        get { return m_type; }
    }
}
