using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementIndicator : MonoBehaviour
{
    [SerializeField] private TowerPlacementManager manager;

    public void PlaceTower() {
        manager.PlaceTower();
    }
}
