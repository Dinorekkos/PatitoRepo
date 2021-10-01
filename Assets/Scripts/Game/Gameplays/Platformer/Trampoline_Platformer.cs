using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Platformer {
    public class Trampoline_Platformer : MonoBehaviour
    {
        #region public methods

        #endregion

        #region public variables

        [SerializeField] private AudioManager audioManager;

        #endregion

        #region private methods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //If added force, dont add again
            if (addedForce)
                return;

            CharacterController_Platformer character = collision.GetComponent<CharacterController_Platformer>();

            if (character != null)
            {
                audioManager.Play("Bounce");
                character.AddVerticalForce(trampolineForce);
                addedForce = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            addedForce = false;
        }
        #endregion

        #region private variables
        [SerializeField]
        private float trampolineForce = 20;

        private bool addedForce = false;
        #endregion
    }
}