using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator_Platformer : MonoBehaviour
{
    #region public methods

    #endregion

    #region public variables
    public float idleTime = 2f;
    public float attackingTime = 0.3f;

    public VentiladorState state;

    public Animator myAnimator;
    #endregion

    #region private methods
    private void Update()
    {
        elapsedTimeState += Time.deltaTime;

        if (state == VentiladorState.Idle)
        {
             UpdateIdle();
        } else if (state == VentiladorState.Attacking)
        {
            UpdateAttacking();
        }
    }

    private void UpdateIdle()
    {
        myAnimator.SetBool(ATTACKING_PARAMETER_NAME, false);

        if (elapsedTimeState >= idleTime)
        {
            state = VentiladorState.Attacking;
            elapsedTimeState = 0f;
        }
    }

    public void UpdateAttacking()
    {
        myAnimator.SetBool(ATTACKING_PARAMETER_NAME, true);

        if (elapsedTimeState >= attackingTime)
        {
            state = VentiladorState.Idle;
            elapsedTimeState = 0f;
        }
    }
    #endregion

    #region private variables
    private float elapsedTimeState = 0f;

    private const string ATTACKING_PARAMETER_NAME = "attacking";
    #endregion
}

public enum VentiladorState
{
    Idle,
    Attacking
}
