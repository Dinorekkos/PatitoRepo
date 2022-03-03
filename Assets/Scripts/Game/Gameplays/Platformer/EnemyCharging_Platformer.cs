using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharging_Platformer : MonoBehaviour
{
    #region public methods

    #endregion

    #region public variables
    
    public float Speed
    {
        get { return _speed; }
        private set { _speed = value; }
    }
    #endregion

    #region private methods
    private void Start()
    {
        state = EnemyChargingState.Waiting;

        VisionRangeTrigger.OnTriggerEnter += OnTriggerEnterVisionRange;
        HurboxTrigger.OnTriggerEnter += OnTriggerEnterHurtbox;
        
    }

    private void OnDestroy()
    {
        VisionRangeTrigger.OnTriggerEnter -= OnTriggerEnterVisionRange;
        HurboxTrigger.OnTriggerEnter -= OnTriggerEnterHurtbox;
    }

    private void Update()
    {
        float movement = 0;

        if (state == EnemyChargingState.Charging)
        {
            movement = -Speed * Time.deltaTime;
        }

        Vector3 targetPosition = transform.position;
        targetPosition.x += movement;
        transform.position = targetPosition;

        //check if enemy is too left
        if (transform.position.x < originX - _maxDistance)
        {
            state = EnemyChargingState.Waiting;
            //Destroy(this.gameObject);
            movement = 0;
            
        }
        
    }

    private void OnTriggerEnterVisionRange(Collider2D collision)
    {
        state = EnemyChargingState.Charging;
    }

    private void OnTriggerEnterHurtbox(Collider2D collision)
    {
        if (collision.GetComponent<HealthEntity>())
        {
            collision.GetComponent<HealthEntity>().MakeDamage(1);
            print("Detect Player");
        }
    }

    #endregion

    #region private variables
    private EnemyChargingState state;

    [SerializeField]
    private float _speed = 0;

    public Collider2D_EventHandler VisionRangeTrigger;
    public Collider2D_EventHandler HurboxTrigger;

    [SerializeField]
    private float _maxDistance = 50f;
    private float originX = 0f;
    #endregion
}

public enum EnemyChargingState
{
    Waiting,
    Charging
}