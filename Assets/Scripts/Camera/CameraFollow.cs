using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    #region public_methods
    public void ShakeCamera()
    {
        MyCamera.DOShakePosition(shakeTime, shakeStrength, shakeVibrato, 30, false);
    }
    #endregion

    #region private_methods
    private void Awake()
    {
        Instance = this;
        OriginalPosition = transform.position;

        //Saving right and left limits
        rightLimitOffset = MyCamera.ViewportToWorldPoint(new Vector2(1f, 0.5f)).x;
        rightLimitOffset -= OriginalPosition.x;

        leftLimitOffset = MyCamera.ViewportToWorldPoint(new Vector2(0f, 0.5f)).x;
        leftLimitOffset -= OriginalPosition.x;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(MyFollowTarget.transform.position.x + followOffset.x, cameraParameters.LeftLimig, cameraParameters.RightLimit),
            Mathf.Clamp(MyFollowTarget.transform.position.y + followOffset.y, cameraParameters.BottonLimit, cameraParameters.TopLimit),
            transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentFollowSpeed, followSmooth);
        OnCameraUpdate?.Invoke();
    }
    #endregion

    #region public_vars
    public static CameraFollow Instance;
    public System.Action OnCameraUpdate;

    public Vector3 OriginalPosition;
    public Vector3 CurrentPosition
    {
        get
        {
            return transform.position;
        }
    }

    public Vector3 MovedDirection
    {
        get
        {
            return CurrentPosition - OriginalPosition;
        }
    }

    public float LeftLimit
    {
        get
        {
            return CurrentPosition.x + leftLimitOffset;
        }
    }

    public float RightLimit
    {
        get
        {
            return CurrentPosition.x + rightLimitOffset;
        }
    }

    public Transform MyFollowTarget
    {
        get
        {
            if (_followTarget == null)
            {
                _followTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
            return _followTarget;
        }
        set
        {
            _followTarget = value;
        }
    }

    public Camera MyCamera
    {
        get
        {
            if (myCamera == null)
            {
                myCamera = GetComponentInChildren<Camera>();
            }
            return myCamera;
        }
    }
    #endregion

    #region private_vars
    [SerializeField]
    private Transform _followTarget;
    [SerializeField]
    private Vector2 followOffset;
    [SerializeField]
    private CameraParameters cameraParameters;

    [SerializeField]
    private float followSmooth;
    private Vector3 currentFollowSpeed;

    [SerializeField]
    private Camera myCamera;
    private float rightLimitOffset;
    private float leftLimitOffset;

    //Shake
    [SerializeField]
    private float shakeTime = 0.2f;
    [SerializeField]
    private float shakeStrength = 2;
    [SerializeField]
    private int shakeVibrato = 10;
    #endregion
}
