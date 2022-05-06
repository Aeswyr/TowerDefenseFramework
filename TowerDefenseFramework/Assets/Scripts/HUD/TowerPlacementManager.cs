using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerPlacementManager : MonoBehaviour {
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject exitButton;

    private Tilemap tilemap;
    private Camera cam;

    [SerializeField]
    private TowerData[] m_unlockedTowers;
    [SerializeField]
    private Tower m_towerPrefab;

    private TowerData targetTowerData = null;

    // Start is called before the first frame update
    void Start() {
        foreach (TowerData towerData in m_unlockedTowers) {
            Instantiate(buttonPrefab, buttonHolder.transform).GetComponent<TowerPlacementButton>().SetTower(towerData);
        }
        cam = FindObjectOfType<Camera>();
        tilemap = FindObjectOfType<Tilemap>();
        targetTowerData = null;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (targetTowerData != null) {
            Vector3 currPos = cam.WorldToScreenPoint(tilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition)));
            placementIndicator.transform.position = currPos;

        }
    }

    public void PlaceTower() {
        Debug.Log("Placing Tower: " + targetTowerData.name);
        if (targetTowerData == null) {
            return;
        }

        // get the potential placement position
        Vector3 potentialTowerPos = cam.ScreenToWorldPoint(Input.mousePosition);

        // TODO: check if a tower already exists on this square
        bool validCell = TilemapManager.instance.IsValidPlacement(potentialTowerPos);

        if (validCell) {
            Debug.Log("Actually creating new tower instance!");
            Tower newTower = Instantiate(m_towerPrefab, tilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition)), m_towerPrefab.transform.rotation);
            newTower.SetFields(targetTowerData);
        }
        else {
            // TODO: handle full cell case
        }

    }

    public void SetPlaceable(TowerData targetTowerData) {
        Debug.Log("SetPlaceable ");
        this.targetTowerData = targetTowerData;
        placementIndicator.SetActive(true);
        exitButton.SetActive(true);
        
        Debug.Log("Is exit button active " + exitButton.activeSelf);
        placementIndicator.GetComponent<Image>().sprite = targetTowerData.Sprite;
    }

    public void RevokePlacable() {
        targetTowerData = null;
        placementIndicator.SetActive(false);
        exitButton.SetActive(false);
    }
}
