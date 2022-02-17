using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerPlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject exitButton;
    private Tilemap tilemap;
    private Camera cam;
    [SerializeField] private GameObject[] towers;
    private GameObject targetTower = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var twr in towers) {
            Instantiate(buttonPrefab, buttonHolder.transform).GetComponent<TowerPlacementButton>().SetTower(twr);
        }
        cam = FindObjectOfType<Camera>();
        tilemap = FindObjectOfType<Tilemap>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetTower != null) {
            Vector3 currPos = cam.WorldToScreenPoint(tilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition)));
            placementIndicator.transform.position = currPos;
            
        }
    }

    public void PlaceTower() {
        if (targetTower == null)
            return;

        // TODO: check if a tower already exists on this square
        bool cellIsEmpty = true;

        if (cellIsEmpty) {
            Instantiate(targetTower, tilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition)), targetTower.transform.rotation);
        }
        else {
            // TODO: handle full cell case
        }

    }

    public void SetPlacable(GameObject tower) {
        targetTower = tower;
        placementIndicator.SetActive(true);
        exitButton.SetActive(true);
        placementIndicator.GetComponent<Image>().sprite = tower.GetComponent<SpriteRenderer>().sprite;
    }

    public void RevokePlacable() {
        targetTower = null;
        placementIndicator.SetActive(false);
        exitButton.SetActive(false);
    }
}
