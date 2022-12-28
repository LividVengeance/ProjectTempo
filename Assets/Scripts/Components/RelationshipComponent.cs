using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RelationshipComponent : MonoBehaviour
{
    public enum SRelationshipType
    {
        Friend, // Can't be damaged : Won't attack
        Neutral, // Can be damaged : Won't attack
        Warry, // Can be damaged : Will attack if attacked
        Hostile, // Cab be damaged : Will Attack on sight
    };

    public enum SFaction
    {
        Player,
        Other,
    };
    [SerializeField] private SFaction BelongingFaction;

    [SerializeField] private SRelationshipType RelationshipToPlayer;
    [SerializeField, Tooltip("When warry, number of hits before moving to hostile state")] private int HitsToProvoke;

    public SRelationshipType GetRelationshipToPlayer() => RelationshipToPlayer;
    public SFaction GetOwningFaction() => BelongingFaction;
}
