using System.Collections;
using UnityEngine;

public class ShootAbility : MonoBehaviour
{
    [SerializeField]
    private float _range = 1.0f;

    [SerializeField]
    private float _attackPower = 1.0f;

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    [Tooltip("Time in seconds between uses of this ability.")]
    private float _cooldown = 5.0f;

    private float _currentRemainingCooldown = 0.0f;

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            _bulletPrefab, transform.position, Quaternion.identity);
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