using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplays.Running {
    public class RunningUI : MonoBehaviour
    {
        #region public methods
        #endregion

        #region public variables
        #endregion

        #region private methods
        private void Update()
        {
            healthText.text = "Health: "+characterRunning.Health;
        }        
        #endregion

        #region private variables
        [SerializeField]
        private CharacterController_Running characterRunning;

        private Text healthText;
        #endregion
    }
}