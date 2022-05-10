using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PhNarwahl;

public class WaveSpawnManager : MonoBehaviour {
    public static WaveSpawnManager instance;

    private bool done = false;
    private int totalSpawners;
    private int finishedSpawners;

    private int totalOncomers;
    private int finishedOncomers;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (this != instance) {
            Debug.Log("Warning! You have multiple TilemapManagers simultaneously. This may result in unexpected behavior.");
        }

        totalSpawners = FindObjectsOfType<WavesSpawner>().Length;
        finishedSpawners = 0;
    }

    void Update ()
    {
        if (done) return;

        if (finishedSpawners >= totalSpawners) {
            if (finishedOncomers >= totalOncomers) {
                Debug.Log("Level is Done!!!!");
                done = true;
            }
        }
    }

    public void SpawnerComplete(int oncomerCount) {
        finishedSpawners++;
        totalOncomers += oncomerCount;
    }

    public void OncomersFinished(int count) {
        finishedOncomers += count;
    }
}