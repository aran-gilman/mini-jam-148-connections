using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class RegenerationAbility : MonoBehaviour
{
    [SerializeField]
    [Tooltip("After taking damage, disable healing for X seconds.")]
    private float _damageCooldown = 5.0f;

    [SerializeField]
    [Tooltip("If not at full health and not on damage cooldown, apply healing every X seconds.")]
    private float _healInterval = 1.0f;

    [SerializeField]
    [Tooltip("How much health to restore per healing instance.")]
    private float _healAmount = 1.0f;

    private Health _health;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.DamageTaken.AddListener(HandleDamageTaken);
    }

    private void OnDisable()
    {
        _health.DamageTaken.RemoveListener(HandleDamageTaken);
    }

    private void HandleDamageTaken(float damageTaken)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(HealAfterCooldown());
    }

    private IEnumerator HealAfterCooldown()
    {
        yield return new WaitForSeconds(_damageCooldown);

        // Heal once immediately after the damage cooldown ends.
        _health.Heal(_healAmount);
        while (_health.CurrentHealth < _health.MaxHealth)
        {
            // Wait then heal so that the coroutine exits immediately after
            // reaching max health.
            yield return new WaitForSeconds(_healInterval);
            _health.Heal(_healAmount);
        }
    }
}
