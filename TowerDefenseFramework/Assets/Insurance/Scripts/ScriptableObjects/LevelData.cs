using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Insurance/LevelData")]
public class LevelData : ScriptableObject {

    [SerializeField] private string levelID;
    [SerializeField] private float p_fire;
    [SerializeField] private float p_storm;
    [SerializeField] private float p_flood;
    [SerializeField] private int numButterflies;
    [SerializeField] private TextAsset gridArrayTA;
    [SerializeField] private int startFunds;
    [SerializeField] private int quarterFunds;
    [SerializeField] private float quarterTime;
    [SerializeField] private float growthPerQuarter;

    [SerializeField] private Vector2 stationPos;

    [SerializeField] private List<UIInsuranceMenu.Coverage> m_availableCoverages;

    public string ID {
        get { return levelID; }
    }
    public float PFire {
        get { return p_fire; }
    }
    public float PStorm {
        get { return p_storm; }
    }
    public float PFlood {
        get { return p_flood; }
    }
    public int NumButterflies {
        get { return numButterflies; }
    }
    public TextAsset GridArrayTA {
        get { return gridArrayTA; }
    }
    public int StartFunds {
        get { return startFunds; }
    }
    public int QuarterFunds {
        get { return quarterFunds; }
    }
    public float QuarterTime {
        get { return quarterTime; }
    }
    public float QuarterGrowth {
        get { return growthPerQuarter; }
    }
    public Vector2 StationPos {
        get { return stationPos; }
    }
    public List<UIInsuranceMenu.Coverage> AvailableCoverages {
        get { return m_availableCoverages; }
    }
}
