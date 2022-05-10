using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhNarwahl {
    public class WavesSpawner : MonoBehaviour {

        [SerializeField]
        private WaveData[] m_waves;
        [SerializeField]
        private int m_interval;

        public Oncomer oncomerPrefab;

        private bool done = false;
        private int totalSpawned = 0;
        private float countdown = 2f;
        private int waveNumber = 0;
        private int enemyNumber = 0;
        private int spawnId = 100;

        void Update ()
        {
            if (done) return;

            if (countdown <= 0f)
            {
                AttemptSpawn();
            }

            countdown -= Time.deltaTime; 
        }
        void AttemptSpawn()
        {
            if (waveNumber >= m_waves.Length) {
                return;
            }

            SpawnOncomer(m_waves[waveNumber].Oncomers[enemyNumber]);
            enemyNumber++;

            int numEnemiesInWave = m_waves[waveNumber].Oncomers.Length;
            if (enemyNumber < numEnemiesInWave) {
                countdown = m_waves[waveNumber].Interval;
            }
            else if (enemyNumber >= numEnemiesInWave) {
                countdown = m_interval;
                waveNumber++;
                enemyNumber = 0;
            }

            if (waveNumber >= m_waves.Length) {
                done = true;
                WaveSpawnManager.instance.SpawnerComplete(totalSpawned);
            }

        }

        void SpawnOncomer (OncomerData oncomerData) {

            Transform transform = gameObject.GetComponent<Transform>();
            Oncomer oncomer = Instantiate(oncomerPrefab, transform.position, transform.rotation);
            oncomer.ApplyOncomerData(oncomerData);
            oncomer.SpawnId = spawnId++;
            totalSpawned++;
        }

    }
}
