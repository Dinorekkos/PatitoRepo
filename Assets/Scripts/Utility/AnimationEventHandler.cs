using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent onStartAnimationEvent;
    public UnityEvent onFinishAnimationEvent;
    public UnityEventNamed[] eventNames;

    public void OnStartAnimation()
    {
        onStartAnimationEvent?.Invoke();
    }

    public void OnFinishAnimation()
    {
        onFinishAnimationEvent?.Invoke();
    }

    public void OnAnimationEventByName(string eventName)
    {
        Debug.Assert(DictionaryEventNames.ContainsKey(eventName), "Event with name: "+eventName+" was not found in dictionary");
        Debug.Assert(dictionaryEventNames[eventName] != null, "Event with name: "+eventName+" called by "+gameObject.name+", but we dont have listener");
        DictionaryEventNames[eventName]?.Invoke();
    }

    private Dictionary<string, UnityEvent> dictionaryEventNames;
    private Dictionary<string, UnityEvent> DictionaryEventNames
    {
        get
        {
            if (dictionaryEventNames == null)
            {
                dictionaryEventNames = new Dictionary<string, UnityEvent>();
                foreach (UnityEventNamed eventNamed in eventNames)
                {
                    Debug.Assert(!dictionaryEventNames.ContainsKey(eventNamed.eventName), "Trying to add event with name: "+eventNamed.eventName+", but was already in dictionary");
                    dictionaryEventNames.Add(eventNamed.eventName, eventNamed.eventCall);
                }
            }
            return dictionaryEventNames;
        }
    }
}

[System.Serializable]
public struct UnityEventNamed
{
    public string eventName;
    public UnityEvent eventCall;
}
