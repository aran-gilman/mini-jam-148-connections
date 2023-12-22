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
}