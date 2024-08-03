using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData : Object
{
    public virtual string GetEventType()
    {
        return "";
    }

    public virtual void SetData(params object[] args)
    {

    }
}