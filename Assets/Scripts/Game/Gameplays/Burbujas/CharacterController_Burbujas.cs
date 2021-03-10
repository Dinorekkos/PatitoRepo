using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

namespace Gameplays.Burbujas
{
    public class CharacterController_Burbujas : MonoBehaviour
    {
        #region public methods
        public void Jump()
        {
            if (CanJump)
            {
                myCharacterController.SetVerticalForce(Mathf.Sqrt(JumpSpeed * Mathf.Abs(myCharacterController.Parameters.Gravity)));
                lastJumpTime = Time.time;
                isJumpMade = true;
                counterJump++;
                //AudioController.Instance.PlayOneShotSoundEffect(SoundKeys.JumpSoundKey);

                myAnimator.SetTrigger(JUMP_ANIM_NAME);
                myAnimator.SetBool(ISGROUNDED_ANIM_NAME, false);
            }
        }

        public void StopJump()
        {
            if (CanStopJumping)
            {
                myCharacterController.SetVerticalForce(StopJumpSpeed);
            }
        }
        #endregion

        #region public variables
        public float JumpSpeed;
        public float MoveSpeed;
        public bool AllwaysGoingRight = false;
        public bool AutomaticJump = true;
        public int MaxJumps = 2;

        public PlayerState state = PlayerState.Idle;

        public bool CanJump
        {
            get
            {
                //if (state == PlayerState.Dying)
                //{
                //    return false;
                //}

                if (myCharacterController.State.IsGrounded)
                {
                    if (state == PlayerState.Idle ||
                        state == PlayerState.Running)
                    {
                        //If automatic, easy jump
                        if (AutomaticJump)
                        {
                            return true;
                        }
                        //Else, we need to check that jump is not made
                        else if(isJumpMade == false)
                        {
                            return true;
                        }

                        return false;
                    }
                }
                //If not grounded, check if we can make another jumpg
                else if (isJumpMade == false && counterJump < MaxJumps)
                {
                    return true;
                }
                return false;
            }
        }

        public bool CanStopJumping
        {
            get
            {
                //    if (state == PlayerState.Dying)
                //    {
                //        return false;
                //    }

                if (Time.time > lastJumpTime + jumpMinTime)
                {
                    if (myCharacterController.Speed.y > StopJumpSpeed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public float StopJumpSpeed
        {
            get
            {
                return -myCharacterController.Parameters.Gravity / 12f;
            }
        }

        public Vector3 Scale
        {
            private set
            {
                transform.localScale = value;
            }
            get
            {
                return transform.localScale;
            }
        }
        #endregion

        #region private methods

        private void Update()
        {
            if (myCharacterController.State.IsGrounded)
            {
                counterJump = 0;
            }

            myAnimator.SetBool(ISGROUNDED_ANIM_NAME, myCharacterController.State.IsGrounded);

            HandleHorizontalMovement();
            HandleVerticalMovement();
        }

        private void HandleHorizontalMovement()
        {
            //if (state == PlayerState.Attacking)
            //    return;

            

            //If horizontal direction is too low, is not relevant
            if (Mathf.Abs(horizontalDirection) < 0.1f)
            {
                horizontalDirection = 0;
                if (state == PlayerState.Running)
                {
                    state = PlayerState.Idle;
                }
            } else
            {
                if (state == PlayerState.Idle)
                {
                    state = PlayerState.Running;
                }
            }

            //Checar si el input del usuario es hacia la derecha o hacia la izquierda
            if (horizontalDirection > 0)
            {
                Scale = facingRightScale;
            } else 
            if (horizontalDirection < 0)
            {
                Scale = facingLeftScale;
            }

            float movementFactor;
            //Select movement factor acceleration depending on character controller state
            if (myCharacterController.State.IsGrounded || myCharacterController.State.UsingConstantMovement)
            {
                movementFactor = myCharacterController.Parameters.SpeedAccelerationOnGround;
            }
            else
            {
                movementFactor = myCharacterController.Parameters.SpeedAccelerationInAir;
            }

            float newHorizontalForce;
            //Add a smooth movement
            newHorizontalForce = Mathf.Lerp(myCharacterController.Speed.x, horizontalDirection * MoveSpeed, Time.deltaTime * movementFactor);
            
            //I has friction
            if (myCharacterController.Friction > 1)
            {
                newHorizontalForce = newHorizontalForce / myCharacterController.Friction;
            }
            //If friction is less than 1, the player slips
            if(myCharacterController.Friction < 1 && myCharacterController.Friction > 0)
            {
                newHorizontalForce = Mathf.Lerp(myCharacterController.Speed.x, newHorizontalForce, Time.deltaTime * myCharacterController.Friction * 10);
            }
            
            //float aliveSpeedMultiplier = 1f;
            //if (state == PlayerState.Dying)
            //{
            //    aliveSpeedMultiplier = Mathf.Clamp01(Mathf.Lerp(1f, 0f, (Time.time - dieTime) / dieSpeedOffTime));
            //}
            //newHorizontalForce *= aliveSpeedMultiplier;

            myCharacterController.SetHorizontalForce(newHorizontalForce);
            //myAnimator.SetFloat(HorozontalSpeedAnimatorName, newHorizontalForce);
        }

        private void HandleVerticalMovement()
        {
            if (isJumpPressed)
            {
                Jump();
            }
            else
            {
                StopJump();
            }

            //If state is jumping but character is grounded, set idle state
            if (state == PlayerState.Jumping && myCharacterController.State.IsGrounded)
            {
                state = PlayerState.Idle;
            }

            //If state is running, but is not grounded, and is not falling, set jumping
            if (state == PlayerState.Running &&
                !myCharacterController.State.IsGrounded &&
                !myCharacterController.State.IsFalling)
                state = PlayerState.Jumping;

            //myAnimator.SetFloat(VerticalSpeedAnimatorName, myCharacterController.Speed.y);
            //myAnimator.SetBool(GroundedAnimatorName, myCharacterController.State.IsCollidingBelow);
        }

        private void OnEnable()
        {
            myAnimator.SetBool(ISMOVING_ANIM_NAME, false);

            LeanTouch.OnFingerDown += HandleFingerDown;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= HandleFingerDown;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        private void HandleFingerDown(LeanFinger finger)
        {
            isJumpPressed = true;
            
            Vector3 fingerViewportPoint = Camera.main.ScreenToViewportPoint(finger.ScreenPosition);

            //If allways going right, horizontal direction is 1
            if (AllwaysGoingRight)
            {
                horizontalDirection = 1;
            }
            else
            {
                if (fingerViewportPoint.x >= 0.5f)
                {
                    horizontalDirection = 1;
                } else
                {
                    horizontalDirection = -1;
                }
            }
        }

        private void HandleFingerUp(LeanFinger finger)
        {
            isJumpPressed = false;
            isJumpMade = false;

            horizontalDirection = 0;
        }
        #endregion

        #region private variables
        [SerializeField]
        private CharacterController2D myCharacterController;

        [SerializeField]
        private float jumpMinTime = 1f;
        
        private float lastJumpTime = -20;

        private bool isJumpPressed = false;

        private float horizontalDirection = 0;

        private Vector3 facingRightScale = new Vector3(1, 1, 1);
        private Vector3 facingLeftScale = new Vector3(-1, 1, 1);

        private bool isJumpMade = false;
        private int counterJump = 0;

        [SerializeField]
        private Animator myAnimator;

        private const string ISMOVING_ANIM_NAME = "isMoving";
        private const string ISGROUNDED_ANIM_NAME = "isGrounded";
        private const string JUMP_ANIM_NAME = "jump";
        #endregion
    }


    public enum PlayerState
    {
        Idle,
        Running,
        Jumping
    }
}