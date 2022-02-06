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

    public Sprite Sprite {
        get { return m_sprite; }
    }
    public List<TileData.WalkType> CanWalkOn {
        get { return m_canWalkOn; }
    }
}
