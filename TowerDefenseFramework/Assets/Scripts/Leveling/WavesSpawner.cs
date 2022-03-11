using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSpawner : MonoBehaviour {

    public Transform enemyprefab;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveNumber = 1;

    void Update ()
    {
        if (countdown<= 0f)
        {
            SpawnWave();
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime; 
    }
    void SpawnWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            spawnEnemy ();

        }
        waveNumber++;

        Debug.Log("Wave Incoming");
    }

    void spawnEnemy ()
    {
        Instantiate(enemyprefab, spawnPoint.position, spawnPoint.rotation);
    }

}
