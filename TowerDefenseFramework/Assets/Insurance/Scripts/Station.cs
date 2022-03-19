using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Station : MonoBehaviour
{
    [SerializeField]
    private HealthManager m_healthManager;

    public void ApplyDamage(float dmg, Oncomer.Type type) {
        m_healthManager.ModifyHealth(-dmg, type);
    }

    public void InitHealth(float startBase, float startFlood, float startFire, float startStorm, float startUmbrella) {
        m_healthManager.InitFields(startBase, startFlood, startFire, startStorm, startUmbrella);
    }
}
