using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(ShootAbility))]
public class AIController : MonoBehaviour
{
    private AIMovement _aiMovement;
    private ShootAbility _shootAbility;

    private void ApproachNearestTarget()
    {
        _aiMovement.enabled = true;
        _aiMovement.SetTarget(new NearestPathfindingTarget(FindAllPotentialTargets()));
    }

    private IEnumerable<Transform> FindAllPotentialTargets()
    {
        Health[] potentialTargets = FindObjectsByType<Health>(FindObjectsSortMode.None);
        List<Transform> targets = new List<Transform>();
        foreach (Health target in potentialTargets)
        {
            FactionAlignment alignment = target.GetComponentInParent<FactionAlignment>();
            if (alignment == null || alignment.Faction == FactionAlignment.EFaction.Enemy)
            {
                continue;
            }
            targets.Add(target.transform);
        }
        return targets;
    }

    private void HandleStructureChange(Structure structure)
    {
        _aiMovement.SetTarget(null);
    }

    private void Awake()
    {
        _aiMovement = GetComponent<AIMovement>();
        _shootAbility = GetComponent<ShootAbility>();

        _aiMovement.enabled = false;
        _shootAbility.enabled = false;
    }

    private void OnEnable()
    {
        Structure.StructureAdded += HandleStructureChange;
        Structure.StructureRemoved += HandleStructureChange;
    }

    private void OnDisable()
    {
        Structure.StructureAdded -= HandleStructureChange;
        Structure.StructureRemoved -= HandleStructureChange;
    }

    private void Update()
    {
        // If there is a target within range, attack it.
        if (_shootAbility.HasTargetsInRange())
        {
            _shootAbility.enabled = true;
            _aiMovement.enabled = false;
        }
        // Otherwise, if we are not moving toward a target, find one and start
        // moving toward it.
        else if (!_aiMovement.HasTarget())
        {
            ApproachNearestTarget();
        }
    }
}
