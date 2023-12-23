using UnityEngine;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(ShootAbility))]
public class AIController : MonoBehaviour
{
    private AIMovement _aiMovement;
    private ShootAbility _shootAbility;

    private Health _target;

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

    private void Awake()
    {
        _aiMovement = GetComponent<AIMovement>();
        _shootAbility = GetComponent<ShootAbility>();

        _aiMovement.enabled = false;
        _shootAbility.enabled = false;
    }

    private void Update()
    {
        // If there is a target within range, attack it.
        if (_shootAbility.HasTargetsInRange())
        {
            _target = null;
            _shootAbility.enabled = true;
            _aiMovement.enabled = false;
        }
        // Otherwise, if we are not moving toward a target, find one and start
        // moving toward it.
        else if (_target == null)
        {
            _target = FindTarget();
            _aiMovement.enabled = true;
            _aiMovement.TargetPosition = _target.transform.position;
        }
    }
}