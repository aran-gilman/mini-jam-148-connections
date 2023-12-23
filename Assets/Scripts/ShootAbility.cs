using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ShootAbility : MonoBehaviour
{
    [SerializeField]
    private float _range = 1.0f;

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    [Tooltip("Child object spawned on awake that displays the connector's range and provides a collider for checking proximity to other connectors.")]
    private GameObject _shootRangePrefab;

    [SerializeField]
    [Tooltip("Time in seconds between uses of this ability.")]
    private float _cooldown = 5.0f;

    private float _currentRemainingCooldown = 0.0f;

    private List<Transform> _targetsInRange = new List<Transform>();

    private FactionAlignment _selfFactionAlignment;
    private Health _selfHealth;

    public bool HasTargetsInRange()
    {
        return _targetsInRange.Count > 0;
    }

    private Transform FindTarget()
    {
        return _targetsInRange
            .OrderBy(t => (transform.position - t.position).sqrMagnitude)
            .FirstOrDefault();
    }

    private void Shoot(Transform target)
    {
        GameObject bullet = Instantiate(
            _bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = (target.transform.position - bullet.transform.position).normalized;

        if (_selfFactionAlignment != null &&
            bullet.TryGetComponent(out DamageInstance damageInstance))
        {
            damageInstance.OwningFaction = _selfFactionAlignment.Faction;
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(_bulletPrefab, "BulletPrefab must be non-null.");
        Assert.IsNotNull(_bulletPrefab.GetComponent<Rigidbody2D>(),
            "BulletPrefab must have a Rigidbody2D component");

        GameObject rangeObject = Instantiate(_shootRangePrefab, transform);

        // The range is a radius and the scale is effectively the diameter,
        // so set the scale to double the range.
        rangeObject.transform.localScale = new Vector3(
            _range * 2.0f, _range * 2.0f, 1.0f);

        // TriggerCallback does not require prior configuration, so if it's
        // missing, we can just add it.
        TriggerCallback triggerCallback = null;
        if (!rangeObject.TryGetComponent(out triggerCallback))
        {
            triggerCallback = rangeObject.AddComponent<TriggerCallback>();
        }
        triggerCallback.TriggerEntered += HandleTriggerEntered;
        triggerCallback.TriggerExited += HandleTriggerExited;

        _selfFactionAlignment = GetComponentInParent<FactionAlignment>();
        _selfHealth = GetComponentInParent<Health>();
    }

    private void HandleTriggerEntered(object sender, Collider2D other)
    {
        // Only target objects that have health.
        // Also ignore colliders that are on the same object as this one.
        Health otherHealth = other.GetComponentInParent<Health>();
        if (otherHealth == null || otherHealth == _selfHealth)
        {
            return;
        }

        // If both this and the other object have factions, and those factions
        // are the same, then the other object is not a valid target.
        if (_selfFactionAlignment != null)
        {
            FactionAlignment otherFaction = other.GetComponentInParent<FactionAlignment>();
            if (otherFaction != null &&
                otherFaction.Faction == _selfFactionAlignment.Faction)
            {
                return;
            }
        }

        _targetsInRange.Add(otherHealth.transform);
    }

    private void HandleTriggerExited(object sender, Collider2D other)
    {
        Health otherHealth = other.GetComponentInParent<Health>();
        if (otherHealth != null)
        {
            _targetsInRange.Remove(otherHealth.transform);
        }
    }

    private void Update()
    {
        if (_currentRemainingCooldown <= 0.0f)
        {
            Transform target = FindTarget();
            if (target != null)
            {
                Shoot(target);
                _currentRemainingCooldown = _cooldown;
            }
        }
        else
        {
            _currentRemainingCooldown -= Time.deltaTime;
        }
    }
}