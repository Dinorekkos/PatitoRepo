using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Burbujas
{
    public class BubbleSpawner_Burbujas : MonoBehaviour
    {
        #region public methods

        #endregion

        #region public variables

        #endregion

        #region private methods
        //Initialize spawning
        private void Start()
        {
            //Start listening spawn point states
            foreach (BubbleSpawnpoint_Burbujas spawnPoint in _spawnPoints)
            {
                spawnPoint.OnStateChange += OnSpawnPointStateChange;
            }

            //Start spawn process
            StartCoroutine(SpawnProcess());
        }

        private IEnumerator SpawnProcess()
        {
            //Wait for delay
            yield return new WaitForSeconds(_startDelay);

            //Start spawn process loop
            while (_spawningEnemies)
            {
                float spawnRate = Random.Range(_spawnRatioRange.x, _spawnRatioRange.y);
                //Wait for spawn rate
                yield return new WaitForSeconds(spawnRate);

                int indexSpawnPoint = -1;
                bool foundSpawner = false;
                //while we cant find a spawner
                while (foundSpawner == false)
                {
                    if (_spawnPoints.Count > 0)
                    {
                        indexSpawnPoint = Random.Range(0, _spawnPoints.Count);
                    }

                    //If we find an valid index
                    if (indexSpawnPoint >= 0)
                    {
                        foundSpawner = true;
                    }
                    yield return null;
                }

                //With valid spawner, spawn an enemy
                SpawnBubble(_spawnPoints[indexSpawnPoint]);
            }

            //Stop listening spawn point states
            foreach (BubbleSpawnpoint_Burbujas spawnPoint in _spawnPoints)
            {
                spawnPoint.OnStateChange -= OnSpawnPointStateChange;
            }
        }

        private void SpawnBubble(BubbleSpawnpoint_Burbujas spawnPoint)
        {
            Vector3 spawnPosition = spawnPoint.GetSpawnPosition();

            //Instantiate enemy
            GameObject spawned = GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, this.transform);
            BubbleController_Burbujas enemyController = spawned.GetComponent<BubbleController_Burbujas>();
            enemyController.Initialize();

            //Start spawnpoint cooldown 
            spawnPoint.UseSpawnPoint();

            //Remove spawner from possible spawn points
            _spawnPoints.Remove(spawnPoint);
        }

        private void OnSpawnPointStateChange(BubbleSpawnpoint_Burbujas spawnPoint)
        {
            //If spawnpoint is active again
            if (spawnPoint.IsActive)
            {
                //Add spawner to possible spawn points again
                _spawnPoints.Add(spawnPoint);
            }
        }
        #endregion

        #region private variables
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private Vector2 _spawnRatioRange = new Vector2(1, 2);
        [SerializeField]
        private float _startDelay = 0f;

        [SerializeField]
        private List<BubbleSpawnpoint_Burbujas> _spawnPoints = new List<BubbleSpawnpoint_Burbujas>();

        private bool _spawningEnemies = true;
        #endregion
    }
}