using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhNarwahl;
using pHAnalytics;

public class WaveSpawnManager : MonoBehaviour {
    public static WaveSpawnManager instance;

    private Destination moat;

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

        moat = FindObjectOfType<Destination>();
        totalSpawners = FindObjectsOfType<WavesSpawner>().Length;
        finishedSpawners = 0;
    }

    void Update ()
    {
        if (done) return;

        if (finishedSpawners >= totalSpawners) {
            if (finishedOncomers >= totalOncomers) {
                var currentScene = SceneManager.GetActiveScene().name;
                FirebaseUtil.LevelEnd(currentScene, moat.getPH(), moat.getVolume());
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