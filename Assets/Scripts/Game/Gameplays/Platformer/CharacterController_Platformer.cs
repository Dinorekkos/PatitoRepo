using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Platformer
{
    public class CharacterController_Platformer : MonoBehaviour
    {

        public Collider2D playerCollider;
        public bool canMove;


        #region public methods
        public void Jump()
        {
            Jump(JumpSpeed);
        }

        public void Jump(float jumpSpeed)
        {
            if (CanJump)
            {
                myCharacterController.SetVerticalForce(Mathf.Sqrt(jumpSpeed * Mathf.Abs(myCharacterController.Parameters.Gravity)));
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
                //Debug.Log("STOP VERTICAL FORCE");
                myCharacterController.SetVerticalForce(StopJumpSpeed);
            }
        }

        public void AddVerticalForce(float force)
        {
            //Reset jump confim, cant cancel jump and set vertical force to zero
            tryCancelJump = false;
            CharacterController.SetVerticalForce(0f);

            //Add vertical force works like a normal jump
            lastJumpTime = Time.time;
            if (isJumpPressed)
            {
                CharacterController.AddVerticalForce(force * TrampolineJumpingMultiplier);
            } else
            {
                CharacterController.AddVerticalForce(force);
            }
            
            //AudioController.Instance.PlayOneShotSoundEffect(SoundKeys.JumpSoundKey);
            myAnimator.SetTrigger(JUMP_ANIM_NAME);
            myAnimator.SetBool(ISGROUNDED_ANIM_NAME, false);
        }
        
        #endregion

        #region public variables
        public PlayerState state = PlayerState.Idle;
        
        public float JumpSpeed;
        public float MoveSpeed;
        public float TrampolineJumpingMultiplier = 1.1f;
        
        public int MaxJumps = 2;

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
                        //If jump is not made, we can jump
                        if (isJumpMade == false)
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

        public CharacterController2D CharacterController
        {
            get
            {
                return myCharacterController;
            }
        }
        #endregion

        #region private methods

        private void Update()
        {
            if (myCharacterController.State.IsGrounded)
            {
                counterJump = 0;

                //If grounded, and we are not pressing jump, cant cancel jump
                if (!isJumpPressed)
                    tryCancelJump = false;
            }

            myAnimator.SetBool(ISGROUNDED_ANIM_NAME, myCharacterController.State.IsGrounded);
            
            if(canMove)
            HandleInput();
            
            HandleHorizontalMovement();
            HandleVerticalMovement();
        }

        private void HandleInput()
        {
            //Save current horizontal direction
            horizontalDirection = myJoystick.Horizontal;
            verticalDirection = myJoystick.Vertical;

            #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A))
            {
                horizontalDirection--;
                if (horizontalDirection < -1)
                    horizontalDirection = -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                horizontalDirection = 1;
                if (horizontalDirection > 1)
                    horizontalDirection = 1;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumpPressed = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumpPressed = false;
                isJumpMade = false;
                tryCancelJump = true;
            }
#endif
        }

        private void HandleHorizontalMovement()
        {
            //If horizontal direction is too low, is not relevant
            if (Mathf.Abs(horizontalDirection) < 0.1f)
            {
                horizontalDirection = 0;
                if (state == PlayerState.Running)
                {
                    state = PlayerState.Idle;
                }
            }
            else
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
            }
            else
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
            if (myCharacterController.Friction < 1 && myCharacterController.Friction > 0)
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

            if (Mathf.Abs(newHorizontalForce) > 0.01f)
            {
                myAnimator.SetBool(ISMOVING_ANIM_NAME, true);
            }
            else
            {
                myAnimator.SetBool(ISMOVING_ANIM_NAME, false);
            }
        }

        private void HandleVerticalMovement()
        {
            if (isJumpPressed)
            {
                Jump();
            }

            if (tryCancelJump)
            {
                StopJump();
            }

            //If state is jumping, but character is grounded, set idle state
            if (state == PlayerState.Jumping && myCharacterController.State.IsGrounded)
            {
                state = PlayerState.Idle;
            }

            //If state is running or idle, but is not grounded, and is not falling, set jumping
            if ((state == PlayerState.Running || state == PlayerState.Idle) &&
                !myCharacterController.State.IsGrounded &&
                !myCharacterController.State.IsFalling)
            {
                state = PlayerState.Jumping;
            }
            
            //myAnimator.SetFloat(VerticalSpeedAnimatorName, myCharacterController.Speed.y);
            //myAnimator.SetBool(GroundedAnimatorName, myCharacterController.State.IsCollidingBelow);
        }

        private void OnEnable()
        {
            myJumpButton.OnPointerDownEvent += HandleJumpButtonDown;
            myJumpButton.OnPointerUpEvent += HandleJumpButtonUp;
        }

        private void OnDisable()
        {
            myJumpButton.OnPointerDownEvent -= HandleJumpButtonDown;
            myJumpButton.OnPointerUpEvent -= HandleJumpButtonUp;
        }

        private void HandleJumpButtonDown()
        {
            isJumpPressed = true;
        }

        private void HandleJumpButtonUp()
        {
            isJumpPressed = false;
            isJumpMade = false;
            tryCancelJump = true;
        }


        private void DetectCollidingsSides()
        {
            //playerCollider.GetContacts(ContactPoint2D.collider)   
        }

        

        #endregion

        #region private variables
        [SerializeField]
        private CharacterController2D myCharacterController;
        [SerializeField]
        private Joystick myJoystick;
        [SerializeField]
        private ButtonEvent myJumpButton;

        [SerializeField]
        private float jumpMinTime = 1f;

        private float lastJumpTime = -20;

        private bool isJumpPressed = false;

        private float horizontalDirection = 0;
        private float verticalDirection = 0;

        private Vector3 facingRightScale = new Vector3(1, 1, 1);
        private Vector3 facingLeftScale = new Vector3(-1, 1, 1);

        private bool isJumpMade = false;
        private int counterJump = 0;
        private bool tryCancelJump = false;

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
        Jumping,
        Colliding
    }
}
