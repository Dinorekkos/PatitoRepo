﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator_Platformer : MonoBehaviour
{
    
    [SerializeField] private bool isActive;
    #region public methods

    #endregion
    
    #region public variables
    public float idleTime = 2f;
    public float attackingTime = 0.3f;

    public VentiladorState state;

    public Animator myAnimator;

    public Collider2D_EventHandler HurtboxTrigger;
    #endregion

    [SerializeField]private AudioSource audioSource;
    [SerializeField] private GameController gameController;
    
    
    #region private methods
    private void Start()
    {
        HurtboxTrigger.OnTriggerStay += OnTriggerStayHurtbox;
    }

    private void OnDestroy()
    {
        HurtboxTrigger.OnTriggerStay -= OnTriggerStayHurtbox;
    }

    private void Update()
    {
        if (gameController.GameFinished)
        {
            audioSource.enabled = false;
        }
        
        elapsedTimeState += Time.deltaTime;

        if (state == VentiladorState.Idle)
        {
            isActive = false;
            if (!isActive)
            {
                audioSource.volume = 0;
                UpdateIdle();
            }
        } else if (state == VentiladorState.Attacking)
        {
            isActive = true;
            if (isActive)
            {
                audioSource.volume = 1;
                UpdateAttacking();
            }
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
    
    private void UpdateAttacking()
    {
        
        myAnimator.SetBool(ATTACKING_PARAMETER_NAME, true);

        if (elapsedTimeState >= attackingTime)
        {
            state = VentiladorState.Idle;
            
            elapsedTimeState = 0f;
        }
    }

    private void OnTriggerStayHurtbox(Collider2D collision)
    {
        //if ventilator is not attacking, cant make damage
        if (state != VentiladorState.Attacking)
            return;

        if (collision.GetComponent<HealthEntity>())
        {
            collision.GetComponent<HealthEntity>().MakeDamage(1);
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
