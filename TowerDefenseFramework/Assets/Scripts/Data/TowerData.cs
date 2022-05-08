using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] private Tower.TargetStrategy m_targetStratgey = Tower.TargetStrategy.First;
    [SerializeField] private bool m_ignoreFullOncomers = false;
    [SerializeField] private Color m_color;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private Oncomer.Type[] m_oncomerTargets;
    [SerializeField] private float m_shootSpeed = 1f;
    [SerializeField] private float m_radius = 3f;
    [SerializeField] private string m_projectileSoundID = "projectile-default";
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private float m_projectileVolume;
    [SerializeField] private float m_projectilePh;
    [SerializeField] private int m_cost;

    public Tower.TargetStrategy TargetStrategy {
        get { return m_targetStratgey; }
    }
    public bool IgnoreFullOncomers {
        get { return m_ignoreFullOncomers; }
    }
    public Color Color {
        get { return m_color; }
    }
    public Sprite Sprite {
        get { return m_sprite; }
    }
    public Oncomer.Type[] OncomerTargets {
        get { return m_oncomerTargets; }
    }
    public float ShootSpeed {
        get { return m_shootSpeed; }
    }
    public float Radius {
        get { return m_radius; }
    }
    public string ProjectileSoundID {
        get { return m_projectileSoundID; }
    }
    public GameObject ProjectilePrefab {
        get { return m_projectilePrefab; }
    }
    public float ProjectileVolume {
        get { return m_projectileVolume; }
    }
    public float ProjectilePh {
        get { return m_projectilePh; }
    }
    public int Cost {
        get { return m_cost; }
    }
}
