using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct CameraParameters
{
    #region public_vars
    public bool HasLeftLimit;
    public bool HasRightLimit;
    public bool HastUpLimit;
    public bool HastDownLimit;

    public float LeftLimig
    {
        get
        {
            if (!HasLeftLimit)
                return Mathf.NegativeInfinity;
            return _leftLimit;
        }
    }

    public float RightLimit
    {
        get
        {
            if (!HasRightLimit)
                return Mathf.Infinity;
            return _rightLimit;
        }
    }

    public float TopLimit
    {
        get
        {
            if (!HastUpLimit)
                return Mathf.Infinity;
            return _upLimit;
        }
    }

    public float BottonLimit
    {
        get
        {
            if (!HastDownLimit)
                return Mathf.NegativeInfinity;
            return _downLimit;
        }
    }
    #endregion

    #region private_vars
    [SerializeField]
    private float _leftLimit;
    [SerializeField]
    private float _upLimit;
    [SerializeField]
    private float _rightLimit;
    [SerializeField]
    private float _downLimit;
    #endregion
}
