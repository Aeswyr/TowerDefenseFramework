using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementButton : MonoBehaviour
{
    [SerializeField] private Image image;
    private Tower.Type towerType;
    private TowerPlacementManager manager;
    void Awake() {
        manager = transform.parent.parent.GetComponent<TowerPlacementManager>();
    }

    public void SetTower(Tower.Type type) {
        TowerData data = GameDB.instance.GetTowerData(type);
        towerType = data.Type;
        image.sprite = data.Sprite;
    }

    public void SetPlacable() {
        manager.SetPlacable(towerType);
    }
}
