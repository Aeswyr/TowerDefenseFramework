using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WaveData : ScriptableObject {
    [SerializeField]
    private OncomerData[] m_oncomers;
    [SerializeField]
    private int m_interval;

    public OncomerData[] Oncomers {
        get { return m_oncomers; }
    }

    public int Interval {
        get { return m_interval; }
    }
}