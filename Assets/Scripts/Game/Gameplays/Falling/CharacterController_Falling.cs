using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class CharacterController_Falling : MonoBehaviour
{
    #region public methods
    public Vector3 GetScreenToWorldPosition(Vector2 screenPosition)
    {
        Vector3 result = _targetPosition;
        Vector3 tempPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (CanFollowXAxis)
            result.x = tempPosition.x;

        if (CanFollowYAxis)
            result.y = tempPosition.y;

        result.z = _targetZPosition;

        return result;
    }
    #endregion

    #region public variables
    public bool CanFollowXAxis
    {
        get { return _followXAxis; }
    }

    public bool CanFollowYAxis
    {
        get { return _followYAxis; }
    }

    public float Health
    {
        get { return _currentHealth; }
        private set { _currentHealth = value; }
    }
    #endregion

    #region private methods
    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
    }

    private void Start()
    {
        //On initialization, set current position as target position
        _targetPosition = transform.position;

        _currentHealth = _maxHealth;

        _myAnimator.SetBool(FALLING_ANIMATOR_PARAM_NAME, true);
    }

    private void Update()
    {
        //Every frame set position to target position
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _followSpeed, _followSmoothTime, _maxSpeed);
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        _targetPosition = GetScreenToWorldPosition(finger.ScreenPosition);

        //Debug.Log("Finger update, screenPos: " + finger.ScreenPosition + ", World position: " + finger.GetWorldPosition(0)+", targetPosition: "+_targetPosition);
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("Ontrigger enter2D, othe: " + other.gameObject.name);
    //    //If collider tag is an enemy, handle enemy damage
    //    if (other.tag == "Enemy")
    //    {
    //        //Try to find enemy controller
    //        EnemyController_Mosquitoes enemyController = other.GetComponent<EnemyController_Mosquitoes>();
    //        //If enemy controller is detected
    //        if (enemyController != null)
    //        {
    //            Health -= 10;
    //        }
    //    }
    //}
    #endregion

    #region private variables
    [SerializeField]
    private float _maxHealth = 100;
    private float _currentHealth = 0;

    [SerializeField]
    private bool _followXAxis = true;
    [SerializeField]
    private bool _followYAxis = true;
    [SerializeField]
    private float _followSmoothTime = 0.1f;
    [SerializeField]
    private float _maxSpeed = 2f;

    [SerializeField]
    private float _targetZPosition = 0f;

    [SerializeField]
    private Animator _myAnimator;

    private Vector3 _targetPosition = Vector3.zero;

    private Vector3 _followSpeed;


    private const string FALLING_ANIMATOR_PARAM_NAME = "falling";
    #endregion
}