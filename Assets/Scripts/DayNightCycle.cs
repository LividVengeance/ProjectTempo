using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    //[SerializeField] private float TimeForFullCycle = 30f;
    //[CustomValueDrawer("CustomDrawer"), SerializeField] private float CurrentTimeOfDay;
    //
    //[ReadOnly, ShowInInspector] private int NumberOfDaysPast; 
    //
    //[SerializeField] private Gradient lightColour;
    //[SerializeField] private Light DayLight;
    //
    //private float CustomDrawer(float Value, GUIContent Label)
    //{
    //    return EditorGUILayout.Slider(Label, Value, 0f, this.TimeForFullCycle);
    //}
    //
    //private void Update()
    //{
    //    if (CurrentTimeOfDay >= TimeForFullCycle)
    //    {
    //        TimeForFullCycle = 0.0f;
    //    }
    //
    //    CurrentTimeOfDay += Time.deltaTime;
    //    if (DayLight)
    //    {
    //        DayLight.GetComponent<Light2D>().color = lightColour.Evaluate(CurrentTimeOfDay * 0.002f);
    //    }
    //}
}
