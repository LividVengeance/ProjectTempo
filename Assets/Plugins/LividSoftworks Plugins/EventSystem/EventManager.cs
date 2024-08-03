using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


public delegate void FEventDelegateHanele(EventData InEventData);

public class FEventListenerHandle
{
    public FEventListenerHandle(string InEventType, FEventDelegateHanele InDeleageteHandle)
    {
        EventType = InEventType;
        EventDelegateHandles.Add(InDeleageteHandle);
    }

    public  bool IsValid()
    {
        return EventType.Length > 0;
    }

    public string EventType;
    public List<FEventDelegateHanele> EventDelegateHandles = new List<FEventDelegateHanele>();
}


public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    public delegate void EventDeleagete(EventData InEventData);

    // Container of funcation pointers to call back for specified event types
    private Dictionary<string, FEventListenerHandle> Listeners = new Dictionary<string, FEventListenerHandle>();

    
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (EventManager)FindObjectOfType(typeof(EventManager));
            }
            return instance;
        }
    }

    public void FireEvent(EventData InEventData)
    {
        FEventListenerHandle EventListenerHandle;
        Listeners.TryGetValue(InEventData.GetEventType(), out EventListenerHandle);

        foreach (FEventDelegateHanele Delegate in EventListenerHandle.EventDelegateHandles)
        {
            if (Delegate != null)
            {
                Delegate.Invoke(InEventData);
            }
        }
    }

    public void FireEvent<EventDataType>(params object[] args) where EventDataType: EventData
    {
        var NewEventData = (EventDataType)Activator.CreateInstance(typeof(EventDataType));
        NewEventData.SetData(args);
        FireEvent((EventDataType)NewEventData);
    }

    public FEventListenerHandle AddListener<EventDataType>(FEventDelegateHanele InDelegateHandle) where EventDataType : EventData
    {
        EventDataType NewEventData = (EventDataType)Activator.CreateInstance(typeof(EventDataType));
        string EventType = NewEventData.GetEventType();

        FEventListenerHandle Handle = null;
        Listeners.TryGetValue(EventType, out Handle);
        if (Handle != null && Handle.IsValid())
        {
            Handle.EventDelegateHandles.Add(InDelegateHandle);
        }
        else
        {
            FEventListenerHandle NewEventHandle = new FEventListenerHandle(EventType, InDelegateHandle);
            Listeners.Add(EventType, NewEventHandle);
            Handle = NewEventHandle;
        }

        NewEventData = null;
        return Handle;
    }
}