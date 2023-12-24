using UnityEngine;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(ShootAbility))]
public class AIController : MonoBehaviour
{
    private AIMovement _aiMovement;
    private ShootAbility _shootAbility;

    private Health _approachTarget;

    private Health FindTarget()
    {
        Health[] potentialTargets = FindObjectsByType<Health>(FindObjectsSortMode.None);
        Health closestTarget = null;
        float closestSqrDistance = float.PositiveInfinity;
        foreach (Health target in potentialTargets)
        {
            FactionAlignment alignment = target.GetComponentInParent<FactionAlignment>();
            if (alignment == null || alignment.Faction == FactionAlignment.EFaction.Enemy)
            {
                continue;
            }

            float sqrDistance =
                (target.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    private void ResetTarget(Structure structure)
    {
        _approachTarget = null;
    }

    private void ApproachNearestTarget()
    {
        _approachTarget = FindTarget();
        _aiMovement.enabled = true;
        _aiMovement.TargetPosition = _approachTarget.transform.position;
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
        Structure.StructureAdded += ResetTarget;
        Structure.StructureRemoved += ResetTarget;
    }

    private void OnDisable()
    {
        Structure.StructureAdded -= ResetTarget;
        Structure.StructureRemoved -= ResetTarget;
    }

    private void Update()
    {
        // If there is a target within range, attack it.
        if (_shootAbility.HasTargetsInRange())
        {
            _approachTarget = null;
            _shootAbility.enabled = true;
            _aiMovement.enabled = false;
        }
        // Otherwise, if we are not moving toward a target, find one and start
        // moving toward it.
        else if (_approachTarget == null)
        {
            ApproachNearestTarget();
        }
    }
}
