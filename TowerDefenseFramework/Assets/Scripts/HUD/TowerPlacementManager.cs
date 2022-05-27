using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using PhNarwahl;
using pHAnalytics;

public class TowerPlacementManager : MonoBehaviour {
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject exitButton;

    private Tilemap tilemap;
    private GridLayout gridLayout;
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
        gridLayout = tilemap.GetComponentInParent<GridLayout>();
        targetTowerData = null;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (targetTowerData != null) {

            Vector3 inputPos = Input.mousePosition;
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(inputPos);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
            Vector3 cellWorldPos = gridLayout.CellToWorld(cellPos);

            placementIndicator.transform.position = cellWorldPos;

        }
    }

    public void PlaceTower() {
        Debug.Log("Placing Tower after click. " + targetTowerData.name);
        if (targetTowerData == null) {
            return;
        }

        // get the potential placement position
        Vector3 potentialTowerPos = cam.ScreenToWorldPoint(Input.mousePosition);

        // TODO: check if a tower already exists on this square
        bool validCell = TilemapManager.instance.IsValidPlacement(potentialTowerPos);

        Debug.Log("Is valid cell? " + validCell);

        if (validCell) {
            Tower newTower = Instantiate(m_towerPrefab, tilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition)), m_towerPrefab.transform.rotation);
            newTower.ApplyTowerData(targetTowerData);

            var currentScene = SceneManager.GetActiveScene().name;
            FirebaseUtil.TowerPlaced(currentScene, potentialTowerPos, targetTowerData.name);
        }
        else {
            // TODO: handle full cell case
        }

    }

    public void SetPlaceable(TowerData targetTowerData) {
        Debug.Log("Setting placable! " + targetTowerData.name);
        this.targetTowerData = targetTowerData;
        placementIndicator.SetActive(true);
        exitButton.SetActive(true);
        placementIndicator.GetComponent<Image>().sprite = targetTowerData.Sprite;
        placementIndicator.GetComponent<Image>().color = PhIndicator.GetColor(targetTowerData.ProjectilePh);
    }

    public void RevokePlacable() {
        targetTowerData = null;
        placementIndicator.SetActive(false);
        exitButton.SetActive(false);
    }
}
