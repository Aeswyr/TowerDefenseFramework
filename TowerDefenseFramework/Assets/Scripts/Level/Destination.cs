using UnityEngine;
using PhNarwahl.pH;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Destination : MonoBehaviour, HasPh {

   //pH variables
    [SerializeField] private float m_molOH = 0;
    [SerializeField] private float m_molH = 0;
    [SerializeField] private float m_volume = 2000;
    [SerializeField] private float m_minAcceptablePh = 0;
    [SerializeField] private float m_maxAcceptablePh = 14;

    public float getPH() {
        return pH.getPH(m_volume, m_molH, m_molOH);
    }

    public float MinAcceptablePh {
        get { return m_minAcceptablePh; }
    }

    public float MaxAcceptablePh {
        get { return m_maxAcceptablePh; }
    }

    private void Awake() {

    }

    // Used by Nexus when instantiating
    public void ManualAwake() {

    }

    private void Update() {

    }

    public void MixSolution(float volume, float molH, float molOH) {
        m_volume += volume;
        m_molH += molH;
        m_molOH += molOH;
    }
}