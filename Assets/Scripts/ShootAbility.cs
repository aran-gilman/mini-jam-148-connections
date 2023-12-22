using UnityEngine;
using UnityEngine.Assertions;

public class ShootAbility : MonoBehaviour
{
    [SerializeField]
    private float _range = 1.0f;

    [SerializeField]
    private float _attackPower = 1.0f;

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    [Tooltip("Child object spawned on awake that displays the connector's range and provides a collider for checking proximity to other connectors.")]
    private GameObject _shootRangePrefab;

    [SerializeField]
    [Tooltip("Time in seconds between uses of this ability.")]
    private float _cooldown = 5.0f;

    private float _currentRemainingCooldown = 0.0f;

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            _bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // TODO: Replace placeholder with velocity based on speed & target
        // direction
        rb.velocity = Vector2.up;
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
    }

    private void HandleTriggerEntered(object sender, Collider2D other)
    {
        Debug.Log($"Trigger entered: {other.name}");
    }

    private void HandleTriggerExited(object sender, Collider2D other)
    {

    }

    private void Update()
    {
        _currentRemainingCooldown -= Time.deltaTime;
        if (_currentRemainingCooldown <= 0.0f)
        {
            Shoot();
            _currentRemainingCooldown = _cooldown;
        }
    }
}