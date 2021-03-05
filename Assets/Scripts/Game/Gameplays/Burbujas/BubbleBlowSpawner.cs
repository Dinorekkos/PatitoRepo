using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

namespace Gameplays.Burbujas
{
    public class BubbleBlowSpawner : MonoBehaviour
    {
        #region public methods
        public void SpawnBubble(BubbleSpawnpoint_Burbujas spawnPoint)
        {
            Vector3 spawnPosition = spawnPoint.GetSpawnPosition();

            //Instantiate
            GameObject spawned = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity, this.transform);
            BubbleController_Burbujas bubbleController = spawned.GetComponent<BubbleController_Burbujas>();
            bubbleController.Initialize();

            //Start spawnpoint cooldown
            spawnPoint.UseSpawnPoint();

            //Remove spawner form possible spawn points
            _spawnPoints.Remove(spawnPoint);

            isSpawned = true;
        }
        
        #endregion

        #region public variables
        public GameObject prefab;
        #endregion

        #region private methods
        private void Start()
        {
            //Start listening spawn point states
            foreach (BubbleSpawnpoint_Burbujas spawnPoint in _spawnPoints)
            {
                spawnPoint.OnStateChange += OnSpawnPointStateChange;
            }
        }
        private void Update()
        {
            //If pressed and not spawned, spawn a bubble
            if (isPressed && !isSpawned)
            {
                if (_spawnPoints != null && _spawnPoints.Count > 0)
                {
                    SpawnBubble(_spawnPoints[0]);
                }
            }
        }

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += HandleFingerDown;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= HandleFingerDown;
            LeanTouch.OnFingerUp -= HandleFingerUp;
        }

        private void HandleFingerDown(LeanFinger finger)
        {
            isPressed = true;
            isSpawned = false;
        }

        private void HandleFingerUp(LeanFinger finger)
        {
            isPressed = false;
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
        private bool isPressed = false;
        private bool isSpawned = false;

        [SerializeField]
        private List<BubbleSpawnpoint_Burbujas> _spawnPoints = new List<BubbleSpawnpoint_Burbujas>();
        #endregion
    }
}
