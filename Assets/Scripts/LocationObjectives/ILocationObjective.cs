using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELocationObjectiveState
{
    // The objective has been completed
    Completed,
    // The objective is currently in progress
    InProgress,
    // The objective is relealed but not currently in progress
    Discovered,
    // The player hasn't come close enough to reveal objective
    Undiscovered,
}

public interface ILocationObjective
{
    private void OnLocationObjectiveAreaEntered()
    {

    }

    private void OnLocationObjectiveAreaExited()
    {

    }

    private void OnCompletedLocationObjective()
    {

    }


}
