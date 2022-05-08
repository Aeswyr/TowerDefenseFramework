using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PhNarwahl;
using PhNarwahl.pH;

[RequireComponent(typeof(Button))]
public class TowerPlacementButton : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private TowerData data;
    private TowerPlacementManager manager;
    private Button m_button;

    void Awake() {
        manager = transform.parent.parent.GetComponent<TowerPlacementManager>();
        m_button = this.GetComponent<Button>();
    }

    public void SetTower(TowerData tower) {
        data = tower;
        image.sprite = data.Sprite;
        image.color = PhIndicator.GetColor(data.ProjectilePh);
    }

    public void SetPlaceable() {
        manager.SetPlaceable(data);
    }
}