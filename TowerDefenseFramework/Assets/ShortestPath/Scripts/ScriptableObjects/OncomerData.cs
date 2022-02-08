using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OncomerData : ScriptableObject
{
    [SerializeField]
    private Sprite m_sprite;
    [SerializeField]
    private List<TileData.WalkType> m_canWalkOn;
    [SerializeField]
    private float m_speed;

    public Sprite Sprite {
        get { return m_sprite; }
    }
    public List<TileData.WalkType> CanWalkOn {
        get { return m_canWalkOn; }
    }
    public float Speed {
        get { return m_speed; }
    }
}
