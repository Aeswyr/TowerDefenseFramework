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

        private float countdown = 2f;
        private int waveNumber = 0;
        private int enemyNumber = 0;

        void Update ()
        {
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
            
            if (enemyNumber >= m_waves[waveNumber].Oncomers.Length) {
                countdown = m_interval;
                waveNumber++;
                enemyNumber = 0;
                return;
            }

            SpawnOncomer(m_waves[waveNumber].Oncomers[enemyNumber]);
            countdown = m_waves[waveNumber].Interval;
            enemyNumber++;
        }

        void SpawnOncomer (OncomerData oncomerData) {
            Transform transform = gameObject.GetComponent<Transform>();
            Oncomer oncomer = Instantiate(oncomerPrefab, transform.position, transform.rotation);
            oncomer.ApplyOncomerData(oncomerData);
        }

    }
}
