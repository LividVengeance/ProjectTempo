using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationObjectiveArea : MonoBehaviour
{
    BoxCollider2D ObjectiveAreaBoxCollider = null;
    List<ILocationObjective> LocationObjectives = new List<ILocationObjective>();

    private HeroCharacter HeroCharacter;

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
        print("Enter Overlap" + InCollision.gameObject);

        if (InCollision.gameObject.GetComponent<HeroCharacter>() != null)
        {
            OnHeroBeginOverlap(InCollision.gameObject.GetComponent<HeroCharacter>());
        }
    }

    private void OnCollisionExit(Collision InCollision)
    {
        print("Exit Overlap" + InCollision.gameObject);

        if (InCollision.gameObject.GetComponent<HeroCharacter>() != null)
        {
            OnHeroEndOverlap(InCollision.gameObject.GetComponent<HeroCharacter>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject + "2D");
    }

    private void OnHeroBeginOverlap(HeroCharacter InHeroCharacter)
    {
        HeroCharacter = InHeroCharacter;
    }

    private void OnHeroEndOverlap(HeroCharacter InHeroCharacter)
    {
        HeroCharacter = null;
    }
}
