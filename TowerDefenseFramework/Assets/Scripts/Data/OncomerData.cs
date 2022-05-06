using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OncomerData : ScriptableObject {
    [SerializeField]
    private Oncomer.Type m_oncomerType;
    [SerializeField]
    private Sprite m_sprite;
    [SerializeField]
    private List<TileData.WalkType> m_canWalkOn;
    [SerializeField]
    private float m_speed;
    [SerializeField]
    private float m_phStart;
    [SerializeField]
    private float m_volumeStart;
    [SerializeField]
    private float m_volumeMax;
    [SerializeField]
    private bool m_movesDiagonal = false;

    public Oncomer.Type Type {
        get { return m_oncomerType; }
    }
    public Sprite Sprite {
        get { return m_sprite; }
    }
    public List<TileData.WalkType> CanWalkOn {
        get { return m_canWalkOn; }
    }
    public float Speed {
        get { return m_speed; }
    }
    public float StartingPh {
        get { return m_phStart; }
    }

    public float StartingVolume {
        get { return m_volumeStart; }
    }

    public float MaxVolume {
        get { return m_volumeMax; }
    }
    public bool MovesDiagonal {
        get { return m_movesDiagonal; }
    }
}