using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventData_Example : EventData
{
    public string StringType;
    public int IntType;

    public override string GetEventType() 
    {
        return "ExampleEventData";
    }

    public override void SetData(params object[] args)
    {
        StringType = (string)args[0];
        IntType = (int)args[1];
    }
}

public class EventExamples : MonoBehaviour
{
    private FEventDelegateHanele EventHandle;

    private void Start()
    {
        EventHandle = OnRecivedEvent;
        EventManager.Instance.AddListener<EventData_Example>(EventHandle);
    }

    private void OnRecivedEvent(EventData InEventData)
    {
        EventData_Example ExampleData = (EventData_Example)InEventData;

        for (int Index = 0; Index < ExampleData.IntType; Index++)
        {
            Debug.Log(ExampleData.StringType);
        }
    }

    private void OnFireEvent()
    {
        EventManager.Instance.FireEvent<EventData_Example>("Some text to display", 3);
    }
}