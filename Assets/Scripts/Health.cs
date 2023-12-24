using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 10.0f;

    [SerializeField]
    private UnityEvent _healthDepleted;
    public UnityEvent HealthDepleted => _healthDepleted;

    private float _currentHealth;

    public void TakeDamage(float attackPower)
    {
        _currentHealth -= attackPower;
        if (_currentHealth <= 0.0f)
        {
            _healthDepleted.Invoke();
        }
    }

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void Profit(int x)
    {
        Global.Money += x;
    }
}
