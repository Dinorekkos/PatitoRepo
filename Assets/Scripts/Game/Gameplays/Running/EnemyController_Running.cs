﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Running
{
    public class EnemyController_Running : MonoBehaviour
    {
        #region public methods
        public void Initialize()
        {
            Speed = _initialSpeed;
            myAnimator.SetBool(ISMOVING_ANIM_NAME, true);
        }
        #endregion

        #region public variables
        //public float Speed
        //{
        //    get { return _speed; }
        //    private set { _speed = value; }
        //}
        public float Speed
        {
            get { return _speed; }
            private set { _speed = value; }
        }
        #endregion

        #region private methods
        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (hasMaxPosition)
            {
                //Reached max position, stop
                if (transform.position.x >= maxHorizontalPosition)
                {
                    return;
                }
            }


            //Add position using current speed
            Vector3 targetPosition = transform.position;
            targetPosition.x += Speed * Time.deltaTime;
            transform.position = targetPosition;
            
            //Add acceleration to current speed
            Speed += _accelSpeed * Time.deltaTime;

            if (Speed > _maxSpeed)
            {
                Speed = _maxSpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SoapController detectedSoap = collision.GetComponent<SoapController>();
            if (detectedSoap != null)
            {
                Speed -= _collisionDecreaseSpeed;
            }
        }
        #endregion

        #region private variables
        //[SerializeField]
        //private Vector2 minMaxSpeed = new Vector2(4, 6);

        //[SerializeField]
        //private float minPosition = -10f;

        //private float _speed = 0;

        [SerializeField]
        private float _initialSpeed = 2;
        [SerializeField]
        private float _maxSpeed = 5;
        [SerializeField]
        private float _accelSpeed = 0.1f;

        [SerializeField]
        private float _collisionDecreaseSpeed = 2f;

        [SerializeField]
        private float _speed = 0;

        [SerializeField]
        private Animator myAnimator;

        private const string ISMOVING_ANIM_NAME = "isMoving";
        private const string ISGROUNDED_ANIM_NAME = "isGrounded";
        private const string JUMP_ANIM_NAME = "jump";

        [SerializeField]
        private bool hasMaxPosition = false;
        [SerializeField]
        private float maxHorizontalPosition = 0;
        #endregion
    }
}