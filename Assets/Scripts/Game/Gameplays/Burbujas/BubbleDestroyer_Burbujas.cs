using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Burbujas {
    public class BubbleDestroyer_Burbujas : MonoBehaviour
    {
        #region public methods
        #endregion

        #region public variables
        #endregion

        #region private methods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hasCollided)
                return;

            if (collision.gameObject.tag == "Player")
            {
                hasCollided = true;

                bubbleController.InstaExplode();
            }
        }
        #endregion

        #region private variables
        [SerializeField]
        private BubbleController_Burbujas bubbleController;

        private bool hasCollided = false;
        
        #endregion
    }
}