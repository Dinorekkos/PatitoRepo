using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Falling
{
    public class EnemySpawner_Falling : MonoBehaviour
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
            foreach (EnemySpawnpoint_Falling spawnPoint in _spawnPoints)
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
                }

                //With valid spawner, spawn an enemy
                SpawnEnemy(_spawnPoints[indexSpawnPoint]);
            }

            //Stop listening spawn point states
            foreach (EnemySpawnpoint_Falling spawnPoint in _spawnPoints)
            {
                spawnPoint.OnStateChange -= OnSpawnPointStateChange;
            }
        }

        private void SpawnEnemy(EnemySpawnpoint_Falling spawnPoint)
        {
            Vector3 spawnPosition = spawnPoint.GetSpawnPosition();

            //Instantiate enemy
            GameObject spawned = GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, this.transform);
            CandyController_Falling enemyController = spawned.GetComponent<CandyController_Falling>();
            enemyController.Initialize();

            //Start spawnpoint cooldown 
            spawnPoint.UseSpawnPoint();

            //Remove spawner from possible spawn points
            _spawnPoints.Remove(spawnPoint);
        }

        private void OnSpawnPointStateChange(EnemySpawnpoint_Falling spawnPoint)
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
        private List<EnemySpawnpoint_Falling> _spawnPoints = new List<EnemySpawnpoint_Falling>();

        private bool _spawningEnemies = true;
        #endregion
    }
}