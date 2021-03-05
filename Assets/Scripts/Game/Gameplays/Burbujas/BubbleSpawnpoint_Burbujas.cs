using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Burbujas
{
    public class BubbleSpawnpoint_Burbujas : MonoBehaviour
    {
        #region public methods
        //Disable spawnpoint for a time
        public void UseSpawnPoint()
        {
            IsActive = false;
            OnStateChange?.Invoke(this);
            StartCoroutine(Cooldown());
        }

        public Vector3 GetSpawnPosition()
        {
            return this.transform.position;
        }
        #endregion

        #region public variables
        public bool IsActive
        {
            get { return _isActive; }
            private set { _isActive = value; }
        }

        public System.Action<BubbleSpawnpoint_Burbujas> OnStateChange;
        #endregion

        #region private methods
        private IEnumerator Cooldown()
        {
            //Wait for cooldown time
            yield return new WaitForSeconds(cooldownTime);
            IsActive = true;
            OnStateChange?.Invoke(this);
        }
        #endregion

        #region private variables
        [SerializeField]
        private float cooldownTime = 2f;

        private bool _isActive = true;
        #endregion
    }
}