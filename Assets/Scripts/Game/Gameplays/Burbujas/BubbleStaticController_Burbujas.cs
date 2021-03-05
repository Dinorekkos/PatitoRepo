using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Gameplays.Burbujas
{
    public class BubbleStaticController_Burbujas : MonoBehaviour
    {
        #region public methods
        public void ExplodeAnimationEvent()
        {
            myCollider.enabled = false;
        }

        public void FinishExplodeAnimationEvent()
        {
            hasExploded = true;
            Destroy(this.gameObject);
        }
        #endregion

        #region public variables
        #endregion

        #region private methods
        private void Awake()
        {
            //Save current position as staic final position
            staticPosition = this.transform.position;

            //use final position, to set initial height, and set transform to initial position
            initialPosition = staticPosition;
            initialPosition.y = initialHeight;
            this.transform.position = initialPosition;
        }

        private void Start()
        {
            this.transform.DOMove(staticPosition, moveAnimationTime)
                .SetDelay(initialDelay)
                .SetEase(easeType);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hasCollided)
                return;

            if (collision.gameObject.tag == "Player")
            {
                hasCollided = true;
                myAnimator.SetTrigger(TRIGGER_EXPLODE_NAME);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (hasCollided)
                return;

            if (collision.gameObject.tag == "Player")
            {
                hasCollided = true;
                myAnimator.SetTrigger(TRIGGER_EXPLODE_NAME);
            }
        }
        #endregion

        #region private variables
        private Vector3 staticPosition;
        private Vector3 initialPosition;

        [SerializeField]
        private float initialDelay = 0f;
        [SerializeField]
        private float moveAnimationTime = 2f;

        private Ease easeType = Ease.Linear;

        [SerializeField]
        private float initialHeight = -10f;

        private bool hasCollided = false;
        private bool hasExploded = false;

        [SerializeField]
        private Animator myAnimator;
        [SerializeField]
        private Collider2D myCollider;

        private const string TRIGGER_EXPLODE_NAME = "explode";
        #endregion
    }
}