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
        VisionRangeTrigger.OnTriggerEnter += OnTriggerEnterHurtbox; 
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnterVisionRange(Collider2D collision)
    {
        state = EnemyChargingState.Charging;
    }

    private void OnTriggerEnterHurtbox(Collider2D collision)
    {

    }

    #endregion

    #region private variables
    private EnemyChargingState state;

    [SerializeField]
    private float _speed = 0;

    public Collider2D_EventHandler VisionRangeTrigger;
    public Collider2D_EventHandler HurboxTrigger;
    #endregion
}

public enum EnemyChargingState
{
    Waiting,
    Charging
}