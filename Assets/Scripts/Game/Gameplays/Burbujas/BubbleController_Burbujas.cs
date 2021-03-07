using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Burbujas
{
    public class BubbleController_Burbujas : MonoBehaviour
    {
        #region public methods
        public void Initialize()
        {
            float targetSpeed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);

            Speed = targetSpeed;
        }

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
        public float Speed
        {
            get { return _speed; }
            private set { _speed = value; }
        }

        public bool initOnStart = false;
        #endregion

        #region private methods
        
        private void Start()
        {
            if (initOnStart)
                Initialize();
        }

        private void Update()
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x += direction.x * Speed * Time.deltaTime;
            targetPosition.y += direction.y * Speed * Time.deltaTime;
            transform.position = targetPosition;
            
            //If x position, is out of xRange, destroy
            if (IsBetweenRange(transform.position.x, xRange) == false)
            {
                Destroy(this.gameObject);
            } else
            //If y position is out of yRange, destroy
            if (IsBetweenRange(transform.position.y, yRange) == false)
            {
                Destroy(this.gameObject);
            }
        }

        //Check if a 'testValue' is beetween range of a Vector2 'range'
        private bool IsBetweenRange(float testValue, Vector2 range)
        {
            return ((testValue >= Mathf.Min(range.x, range.y)) && (testValue <= Mathf.Max(range.x, range.y)));
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
        [SerializeField]
        private Vector2 minMaxSpeed = new Vector2(4, 6);
        [SerializeField]
        private Vector2 direction = new Vector2(0f,1f);

        [SerializeField]
        private Vector2 xRange = new Vector2(-10f, 10f);
        [SerializeField]
        private Vector2 yRange = new Vector2(-10f, 10f);
        
        private float _speed = 0;

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