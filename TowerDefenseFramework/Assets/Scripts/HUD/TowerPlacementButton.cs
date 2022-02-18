using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementButton : MonoBehaviour
{
    [SerializeField] private Image image;
    private GameObject tower;
    private TowerPlacementManager manager;
    void Awake() {
        manager = transform.parent.parent.GetComponent<TowerPlacementManager>();
    }

    // Start is called before the first frame update
    public void SetTower(GameObject tower) {
        this.tower = tower;
        image.sprite = tower.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetPlacable() {
        manager.SetPlacable(tower);
    }
}
