using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HUDScreen : MenuScreen
{
    [Header("Health UI")]
    [SerializeField] private GameObject HealthUIGameobject;

    [Header("Experience UI")]
    [SerializeField] private GameObject ExperienceUIGameobject;
  

    public override bool OnActionDown(string InActionName)
    {
        bool bHandledInput = false;
        return bHandledInput;
    }
}