using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public System.Action OnPointerDownEvent;
    public System.Action OnPointerUpEvent;

    public void HandlePointerDown()
    {
        OnPointerDownEvent?.Invoke();
    }
    public void HandlePointerUp()
    {
        OnPointerUpEvent?.Invoke();
    }
}
