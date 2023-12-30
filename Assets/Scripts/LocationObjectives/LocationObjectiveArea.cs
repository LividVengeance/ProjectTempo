using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationObjectiveArea : MonoBehaviour
{
    BoxCollider2D ObjectiveAreaBoxCollider = null;
    List<ILocationObjective> LocationObjectives = new List<ILocationObjective>();

    private void Start()
    {
        ObjectiveAreaBoxCollider = GetComponent<BoxCollider2D>();
        ObjectiveAreaBoxCollider.enabled = true;
        ObjectiveAreaBoxCollider.isTrigger = true;

        UpdateLocationObjectives();
    }

    private void UpdateLocationObjectives()
    {
        Collider[] OverlappingObjects = Physics.OverlapBox(transform.position, ObjectiveAreaBoxCollider.size / 2);
        foreach (Collider ObjectCollider in OverlappingObjects)
        {
            if (ObjectCollider.gameObject.TryGetComponent(out ILocationObjective LocationObjectiveObject))
            {
                LocationObjectives.Add(LocationObjectiveObject);
            }
        }
    }

    void OnCollisionEnter(Collision InCollision)
    {
        print(InCollision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject + "2D");
    }
}
