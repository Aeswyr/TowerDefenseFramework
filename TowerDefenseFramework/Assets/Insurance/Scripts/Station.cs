using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Station : MonoBehaviour
{
    [SerializeField]
    private HealthBar m_healthBar;

    public void ApplyDamage(float dmg) {
        m_healthBar.ModifyHealth(-dmg);
    }

    public void InitHealth(float startBase, float startInsurance) {
        m_healthBar.InitFields(startBase, startInsurance);
    }
}
