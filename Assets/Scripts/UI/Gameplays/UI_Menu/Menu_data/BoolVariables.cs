using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dino/Variables/Bool")]
public class BoolVariables : ScriptableObject
{
    public string  developerDescription;
    public bool value;

    public void SetValueBool(bool _value)
    {
        value = _value;
        SetDirty();
    }
}
