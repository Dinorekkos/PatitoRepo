using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dino/Variables/Float")]
public class FloatVariables : ScriptableObject
{
    public string  developerDescription;
    public float value;

    public void SetValue(float _value)
    {
        value = _value;
        SetDirty();
    }
}
