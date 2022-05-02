using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsuranceTimer : MonoBehaviour
{
    [SerializeField] private Image m_radial;
    private float m_timeRemaining;

    public Image Radial {
        get { return m_radial; }
    }

    public float TimeRemaining {
        get { return m_timeRemaining; }
    }
}
