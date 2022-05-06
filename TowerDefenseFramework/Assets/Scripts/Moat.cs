using UnityEngine;
using PhNarwahl.pH;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Moat : MonoBehaviour, PhContainer {
    [SerializeField]
    private GameObject m_debugPrefab;
    [SerializeField]
    private GameObject m_debugHolder;

   //pH variables
    [SerializeField]
    private float m_molOH = 0;
    [SerializeField]
    private float m_molH = 0;
    [SerializeField]
    private float m_volume = 2000;

    // public MoatData MoatData {
    //     get; set;
    // }

    public float getPH() {
        return pH.getPH(m_volume, m_molH, m_molOH);
    }

    private void Awake() {
        // this.MoatData = GameDB.instance.GetOncomerData(m_type);

        // ApplyMoatData();
    }

    // Used by Nexus when instantiating
    public void ManualAwake() {
        // ApplyMoatData();
    }

    private void Update() {

    }

    // private void ApplyMoatData() {
    //     m_volume = this.OncomerData.StartingVolume;
    //     m_molH = pH.getAcidMolarity(this.OncomerData.StartingPh);
    //     m_molOH = pH.getBaseMolarity(this.OncomerData.StartingPh);
    // }

    public void MixSolution(float volume, float molH, float molOH) {
        m_volume += volume;
        m_molH += molH;
        m_molOH += molOH;
    }
}